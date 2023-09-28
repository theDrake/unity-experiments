using UnityEngine;

public class FetchSpawnManager : MonoBehaviour {
  private const float _spawnXMin = -22.0f;
  private const float _spawnXMax = 7.0f;
  private const float _spawnY = 30.0f;
  private const float _startDelay = 1.0f;
  private const float _spawnIntervalMin = 1.0f;
  private const float _spawnIntervalMax = 5.0f;

  public GameObject[] balls;

  private float _spawnInterval = _spawnIntervalMin;

  private void Start() {
    InvokeRepeating(nameof(SpawnRandomBall), _startDelay, _spawnInterval);
  }

  private void SpawnRandomBall () {
    Instantiate(balls[Random.Range(0, balls.Length)],
                new(Random.Range(_spawnXMin, _spawnXMax), _spawnY),
                Quaternion.identity);
    _spawnInterval = Random.Range(_spawnIntervalMin, _spawnIntervalMax);
  }
}
