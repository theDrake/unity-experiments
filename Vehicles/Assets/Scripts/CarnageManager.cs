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
  private const float _envRadius = 250.0f;
  private int _numEnemies;
  private const int _numEnemiesMin = 0;
  private const int _numEnemiesMax = 100;
  private bool _useMetricSystem;

  private void Start() {
    _speedText = GameObject.Find("Speed Text").GetComponent<TextMeshProUGUI>();
    _useMetricSystem = false;
    _numEnemies = 20;
    SpawnEnemies();
    SpawnPlayer();
  }

  private void Update() {
    UpdateSpeedText();
  }

  // ABSTRACTION
  private void UpdateSpeedText() {
    _speedText.text = "Speed: ";
    if (_useMetricSystem) {
      _speedText.text += Mathf.RoundToInt(_player.GetSpeed() * 3.6f) + " KPH";
    } else {
      _speedText.text += Mathf.RoundToInt(_player.GetSpeed() * 2.237f) + " MPH";
    }
  }

  private void SpawnPlayer() {
    int prefab = Random.Range(0, _vehiclePrefabs.Length);

    _player = Instantiate<Vehicle>(_vehiclePrefabs[prefab]);
    _player.gameObject.AddComponent<Player>();
    _player.transform.SetPositionAndRotation(GetRandomSpawnPoint(),
                                             _player.transform.rotation);
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
    enemy.transform.SetPositionAndRotation(GetRandomSpawnPoint(),
                                           enemy.transform.rotation);
  }

  private Vector3 GetRandomSpawnPoint() {
    Vector3 spawnPoint = new Vector3(
        Random.Range(-_envRadius / 2 + transform.position.x,
                     _envRadius / 2 + transform.position.x),
        transform.position.y + 1.0f,
        Random.Range(-_envRadius / 2 + transform.position.z,
                     _envRadius / 2 + transform.position.z)
    );

    return spawnPoint;
  }
}
