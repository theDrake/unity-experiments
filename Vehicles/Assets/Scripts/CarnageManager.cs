using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarnageManager : MonoBehaviour {
  // ENCAPSULATION
  public static CarnageManager Instance { get; private set; }
  public readonly Vector3 CenterPoint = new(0.0f, 2.0f, 175.0f);
  [System.Serializable] private class GameData {
    public string PlayerName;
    public Vehicle.VehicleType PlayerVehicleType;
    public int NumEnemies;
  }

  [SerializeField] private Vehicle[] _vehiclePrefabs;
  private Vehicle _player;
  private const int _minEnemies = 0;
  private const int _maxEnemies = 99;
  private const int _defaultNumEnemies = 10;
  private const int _maxNameLength = 20;
  private float _enemySpawnCountdown;
  private const float _enemySpawnDelay = 3.0f; // seconds
  private const float _envRadius = 250.0f;
  private bool _playing;
  private string _saveFile;
  private GameData _gameData;

  private void Awake() {
    if (!Instance) {
      Instance = this;
      DontDestroyOnLoad(gameObject);
      _playing = false;
      _saveFile = Application.persistentDataPath + "/carnage.json";
      LoadData();
    } else {
      Destroy(gameObject);
    }
  }

  private void Update() {
    if (!_playing) {
      return;
    } else if (Input.GetKeyDown(KeyCode.Escape)) {
      ReturnToTitleScreen();
    } else {
      if (_enemySpawnCountdown > 0 &&
          (_enemySpawnCountdown -= Time.deltaTime) < 0) {
        SpawnEnemies();
      }
    }
  }

  // ABSTRACTION
  public void StartGame() {
    SaveData();
    SpawnPlayer();
    SceneManager.LoadScene(1);
    _enemySpawnCountdown = _enemySpawnDelay;
    _playing = true;
  }

  public void CheckForVictory() {
    if (FindAnyObjectByType<Player>().Dead()) {
      return;
    }
    Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

    foreach (Enemy enemy in enemies) {
      if (enemy.Alive()) {
        return;
      }
    }
    Debug.Log("You win!");
    _enemySpawnCountdown = _enemySpawnDelay;
  }

  private void SpawnPlayer() {
    if (_player) {
      Destroy(_player.gameObject);
    }
    _player =
       Instantiate<Vehicle>(_vehiclePrefabs[(int) _gameData.PlayerVehicleType]);
    _player.Type = _gameData.PlayerVehicleType;
    _player.gameObject.AddComponent<Player>();
    MoveToRandomSpawnPoint(_player);
    DontDestroyOnLoad(_player);
  }

  private void SpawnEnemies() {
    Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

    foreach (Enemy enemy in enemies) {
      Destroy(enemy.gameObject);
    }
    for (int i = 0; i < _gameData.NumEnemies; ++i) {
      SpawnEnemy();
    }
  }

  private void SpawnEnemy() {
    int prefab = Random.Range(0, _vehiclePrefabs.Length);
    Vehicle enemy = Instantiate<Vehicle>(_vehiclePrefabs[prefab]);

    enemy.Type = (Vehicle.VehicleType) prefab;
    enemy.gameObject.AddComponent<Enemy>();
    MoveToRandomSpawnPoint(enemy);
  }

  private void MoveToRandomSpawnPoint(Vehicle v) {
    Vector3 spawnPoint = new Vector3(
        Random.Range(-_envRadius / 2 + CenterPoint.x,
                     _envRadius / 2 + CenterPoint.x),
        CenterPoint.y + (v.Type == Vehicle.VehicleType.Airplane ?
                         Random.Range(_envRadius / 10, _envRadius) : 0.5f),
        Random.Range(-_envRadius / 2 + CenterPoint.z,
                     _envRadius / 2 + CenterPoint.z)
    );
    v.transform.SetPositionAndRotation(spawnPoint, v.transform.rotation);
    v.transform.LookAt(CenterPoint);
  }

  public bool OutOfBounds(Vector3 point) {
    return point.y < 0 || Vector3.Distance(point, CenterPoint) > _envRadius;
  }

  public string GetPlayerName() {
    return _gameData.PlayerName;
  }

  public void SetPlayerName(string value) {
    if (value.Length > _maxNameLength) {
      Debug.Log("Excessive name length: " + value.Length);
      _gameData.PlayerName = value[.._maxNameLength];
    } else {
      _gameData.PlayerName = value;
    }
  }

  public Vehicle.VehicleType GetPlayerVehicleType() {
    return _gameData.PlayerVehicleType;
  }

  public void SetPlayerVehicleType(int value) {
    if (value < 0 || value > (int) Vehicle.VehicleType.NumVehicleTypes) {
      Debug.Log("Invalid value for PlayerVehicleType: " + value);
      value = 0;
    }
    _gameData.PlayerVehicleType = (Vehicle.VehicleType) value;
  }

  public int GetNumEnemies() {
    return _gameData.NumEnemies;
  }

  public void SetNumEnemies(int value) {
    if (value < _minEnemies) {
      Debug.Log("Invalid value for NumEnemies: " + value);
      _gameData.NumEnemies = _minEnemies;
    } else if (value > _maxEnemies) {
      Debug.Log("Invalid value for NumEnemies: " + value);
      _gameData.NumEnemies = _maxEnemies;
    } else {
      _gameData.NumEnemies = value;
    }
  }

  public void SaveData() {
    File.WriteAllText(_saveFile, JsonUtility.ToJson(_gameData));
  }

  public void LoadData() {
    if (File.Exists(_saveFile)) {
      string json = File.ReadAllText(_saveFile);
      _gameData = JsonUtility.FromJson<GameData>(json);
    } else {
      _gameData = new GameData();
      _gameData.PlayerName = "";
      _gameData.PlayerVehicleType = 0;
      _gameData.NumEnemies = _defaultNumEnemies;
    }
  }

  private void ReturnToTitleScreen() {
    _playing = false;
    SceneManager.LoadScene(0);
  }
}
