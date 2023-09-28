using UnityEngine;

public class BalloonSpawnManager : MonoBehaviour {
  private const float _spawnDelay = 2.0f;
  private const float _spawnInterval = 1.5f;

  public GameObject[] objectPrefabs;

  private BalloonPlayer _player;

  private void Start() {
    InvokeRepeating(nameof(SpawnObject), _spawnDelay, _spawnInterval);
    _player = GameObject.FindWithTag("Player").GetComponent<BalloonPlayer>();
  }

  private void SpawnObject () {
    if (!_player.gameOver) {
      int i = Random.Range(0, objectPrefabs.Length);

      Instantiate(objectPrefabs[i], new(30, Random.Range(5, 15)),
                  objectPrefabs[i].transform.rotation);
    }
  }
}
