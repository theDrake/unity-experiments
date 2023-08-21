using UnityEngine;
using TMPro;

public class CarnageManager : MonoBehaviour {
  // ENCAPSULATION
  public int NumEnemies {
    get { return _numEnemies; }
    set {
      if (value < _numEnemiesMin) {
        _numEnemies = _numEnemiesMin;
      } else if (value > _numEnemiesMax) {
        _numEnemies = _numEnemiesMax;
      }
      _numEnemies = value;
    }
  }

  [SerializeField] private Vehicle[] _vehiclePrefabs;
  private Vehicle _player;
  private TextMeshProUGUI _speedText;
  private int _numEnemies;
  private const int _numEnemiesMin = 0;
  private const int _numEnemiesMax = 100;
  private const float _envRadius = 250.0f;
  private static Vector3 _center = new(0.0f, 2.0f, 175.0f);

  public static bool OutOfBounds(Vector3 point) {
    return point.y < 0 || Vector3.Distance(point, _center) > _envRadius;
  }

  public static Vector3 GetCenterPoint() {
    return _center;
  }

  private void Start() {
    _speedText = GameObject.Find("Speed Text").GetComponent<TextMeshProUGUI>();
    _numEnemies = 20;
    SpawnEnemies();
    SpawnPlayer();
  }

  private void Update() {
    UpdateSpeedText();
  }

  // ABSTRACTION
  private void UpdateSpeedText() {
    float speed = _player.GetSpeed();

    _speedText.text = "Speed: " + GetMph(speed) + " mph / " + GetKph(speed) +
        " kph";
  }

  private float GetMph(float n) {
    return Mathf.Round(n * 2.237f);
  }

  private float GetKph(float n) {
    return Mathf.Round(n * 3.6f);
  }

  private void SpawnPlayer() {
    int prefab = Random.Range(0, _vehiclePrefabs.Length);

    _player = Instantiate<Vehicle>(_vehiclePrefabs[prefab]);
    _player.gameObject.AddComponent<Player>();
    MoveToRandomSpawnPoint(_player);
  }

  private void SpawnEnemies() {
    for (int i = 0; i < _numEnemies; ++i) {
      SpawnEnemy();
    }
  }

  private void SpawnEnemy() {
    int prefab = Random.Range(0, _vehiclePrefabs.Length);
    Vehicle enemy = Instantiate<Vehicle>(_vehiclePrefabs[prefab]);

    enemy.gameObject.AddComponent<Enemy>();
    MoveToRandomSpawnPoint(enemy);
  }

  private void MoveToRandomSpawnPoint(Vehicle v) {
    Vector3 spawnPoint = new Vector3(
        Random.Range(-_envRadius / 2 + _center.x, _envRadius / 2 + _center.x),
        _center.y + (v.GetComponent<Airplane>() ? Random.Range(_envRadius / 10,
                                                               _envRadius) :
                                                  0.5f),
        Random.Range(-_envRadius / 2 + _center.z, _envRadius / 2 + _center.z)
    );
    v.transform.SetPositionAndRotation(spawnPoint, v.transform.rotation);
    v.transform.LookAt(_center);
  }
}
