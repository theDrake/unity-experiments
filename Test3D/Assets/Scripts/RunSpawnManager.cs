using UnityEngine;

public class RunSpawnManager : MonoBehaviour {
  private readonly Vector3 _spawnPoint = new(25, 0);
  private const float _startDelay = 2.0f;
  private const float _repeatRate = 2.0f;
  private const float _obstacleHeight = 1.8f;

  public GameObject[] obstacles;

  private RunPlayer _player;

  private void Start() {
    InvokeRepeating(nameof(SpawnObstacle), _startDelay, _repeatRate);
    _player = FindAnyObjectByType<RunPlayer>();
  }

  private void SpawnObstacle() {
    if (!_player.gameOver && Random.Range(0, 6) > 0) {
      int i = Random.Range(0, obstacles.Length);

      Instantiate(obstacles[i], _spawnPoint, obstacles[i].transform.rotation);
      if (Random.Range(0, 3) == 0) {
        Instantiate(obstacles[i],
                    new(_spawnPoint.x,
                        _spawnPoint.y + _obstacleHeight,
                        _spawnPoint.z),
                    obstacles[i].transform.rotation);
      }
    }
  }
}
