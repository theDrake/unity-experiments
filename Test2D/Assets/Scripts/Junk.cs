using UnityEngine;

public class Junk : MonoBehaviour {
  private const float _spawnY = -1.0f;
  private const float _spawnRangeX = 4.0f;
  private const float _torqueRange = 1.5f;
  private const float _forceMin = 12.0f;
  private const float _forceMax = 14.0f;

  public ParticleSystem explosion;
  public int points;

  private GameManager _gameManager;
  private Rigidbody _rb;

  private void Start() {
    _gameManager = FindAnyObjectByType<GameManager>();
    _rb = GetComponent<Rigidbody>();
    _rb.AddForce(Vector3.up * Random.Range(_forceMin, _forceMax),
                 ForceMode.Impulse);
    _rb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(),
                  ForceMode.Impulse);
    transform.position = RandomSpawnPoint();
    if (name.Contains("GoodCrate")) {
      points = 5 * Random.Range(1, 7); // 5 to 30
    }
  }

  public void Explode() {
    Instantiate(explosion, transform.position, explosion.transform.rotation);
    Destroy(gameObject);
    if (_gameManager.playing) {
      _gameManager.UpdateScore(points);
    }
  }

  private void OnTriggerEnter(Collider other) {
    Destroy(gameObject);
    if (_gameManager.playing && name.StartsWith("G")) {
      _gameManager.UpdateLives(-1);
    }
  }

  private Vector3 RandomSpawnPoint() {
    return new(Random.Range(-_spawnRangeX, _spawnRangeX), _spawnY);
  }

  private float RandomTorque() {
     return Random.Range(-_torqueRange, _torqueRange);
  }
}
