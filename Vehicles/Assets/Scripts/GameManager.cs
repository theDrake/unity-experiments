using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
  public static GameManager Instance { get; private set; }
  public readonly Vector3 Center = new(0.0f, 2.0f, 175.0f);
  [System.Serializable] private class GameData {
    public Vehicle.VehicleType PlayerVehicleType;
    public int NumEnemies;
    public int NumObstacles;
  }

  [SerializeField] private Vehicle[] _vehiclePrefabs;
  [SerializeField] private GameObject[] _obstaclePrefabs;
  private Vehicle _player;
  private const int _minEnemies = 0;
  private const int _maxEnemies = 99;
  private const int _defaultNumEnemies = 10;
  private const int _minObstacles = 0;
  private const int _maxObstacles = 99;
  private const int _defaultNumObstacles = 10;
  private float _spawnCountdown;
  private const float _initialSpawnDelay = 0.1f; // seconds
  private const float _respawnDelay = 5.0f; // seconds
  private const float _envRadius = 250.0f;
  private const float _spawnRadius = _envRadius * 0.6f;
  private bool _playing;
  private string _saveFile;
  private GameData _gameData;

  private void Awake() {
    if (!Instance) {
      Instance = this;
      DontDestroyOnLoad(gameObject);
      _playing = false;
      _saveFile = Application.persistentDataPath + "/carnage.json";
      _gameData = new GameData {
        PlayerVehicleType = 0,
        NumEnemies = _defaultNumEnemies,
        NumObstacles = _defaultNumObstacles
      };
      LoadData();
    } else {
      Destroy(gameObject);
    }
  }

  private void Update() {
    if (!_playing) {
      return;
    } else if (Input.GetKeyUp(KeyCode.Escape) ||
               Input.GetKeyUp(KeyCode.Backspace)) {
      ReturnToTitleScreen();
    } else if (_spawnCountdown > 0 &&
               (_spawnCountdown -= Time.deltaTime) < 0) {
      SpawnObstacles();
      SpawnEnemies();
    }
  }

  public void StartGame() {
    SaveData();
    SpawnPlayer();
    SceneManager.LoadScene(1);
    _spawnCountdown = _initialSpawnDelay;
    _playing = true;
  }

  public bool Victorious() {
    if (FindAnyObjectByType<Player>().Dead()) {
      return false;
    }
    Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

    foreach (Enemy enemy in enemies) {
      if (enemy.Alive()) {
        return false;
      }
    }
    _spawnCountdown = _respawnDelay;

    return true;
  }

  private void SpawnPlayer() {
    if (_player) {
      Destroy(_player.gameObject);
    }
    _player = SpawnVehicle(_gameData.PlayerVehicleType);
    _player.gameObject.AddComponent<Player>();
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
    CarnageCanvas.SetNumEnemies(_gameData.NumEnemies);
  }

  private void SpawnEnemy() {
    Vehicle enemy = SpawnVehicle(
        (Vehicle.VehicleType) Random.Range(0, _vehiclePrefabs.Length));

    enemy.gameObject.AddComponent<Enemy>();
  }

  private Vehicle SpawnVehicle(Vehicle.VehicleType type) {
    Vehicle v = Instantiate<Vehicle>(_vehiclePrefabs[(int) type]);

    v.Type = type;
    MoveToRandomSpawnPoint(v.gameObject);
    if (type == Vehicle.VehicleType.Airplane) {
      v.transform.SetPositionAndRotation(
        new(v.transform.position.x,
            Random.Range(_envRadius / 10, _spawnRadius),
            v.transform.position.z),
        v.transform.rotation);
    }

    return v;
  }

  private void SpawnObstacles() {
    GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

    foreach (GameObject obstacle in obstacles) {
      Destroy(obstacle);
    }
    for (int i = 0; i < _gameData.NumObstacles; ++i) {
      int type = Random.Range(0, _obstaclePrefabs.Length);
      GameObject obj = Instantiate<GameObject>(_obstaclePrefabs[type]);

      MoveToRandomSpawnPoint(obj);
      if (type == 0) { // boulder
        float multiplier = Random.Range(1.0f, 5.0f);

        obj.transform.localScale *= multiplier;
        obj.GetComponent<Rigidbody>().mass *= multiplier;
        obj.transform.rotation = Random.rotation;
      }
    }
  }

  private void MoveToRandomSpawnPoint(GameObject obj) {
    obj.transform.SetPositionAndRotation(
        new(Random.Range(Center.x - _spawnRadius, Center.x + _spawnRadius),
            Center.y,
            Random.Range(Center.z - _spawnRadius, Center.z + _spawnRadius)),
        obj.transform.rotation);
    obj.transform.LookAt(Center);
  }

  public bool OutOfBounds(Vector3 point) {
    return point.y < 0 || Vector3.Distance(point, Center) > _envRadius;
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

  public int GetNumObstacles() {
    return _gameData.NumObstacles;
  }

  public void SetNumObstacles(int value) {
    if (value < _minObstacles) {
      Debug.Log("Invalid value for NumObstacles: " + value);
      _gameData.NumObstacles = _minObstacles;
    } else if (value > _maxObstacles) {
      Debug.Log("Invalid value for NumObstacles: " + value);
      _gameData.NumObstacles = _maxObstacles;
    } else {
      _gameData.NumObstacles = value;
    }
  }

  public void SaveData() {
    File.WriteAllText(_saveFile, JsonUtility.ToJson(_gameData));
  }

  public void LoadData() {
    if (File.Exists(_saveFile)) {
      string json = File.ReadAllText(_saveFile);
      JsonUtility.FromJsonOverwrite(json, _gameData);
    }
  }

  private void ReturnToTitleScreen() {
    _playing = false;
    SceneManager.LoadScene(0);
  }
}
