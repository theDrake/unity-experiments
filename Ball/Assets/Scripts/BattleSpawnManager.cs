using UnityEngine;

public class BattleSpawnManager : MonoBehaviour {
  public GameObject enemyPrefab, fastEnemyPrefab, bossPrefab, fastBossPrefab;
  public GameObject[] powerupPrefabs;

  private int _wave = 1;
  private const float _spawnRangeX = 7.0f;
  private const float _minY = 5.0f;
  private const float _maxY = 15.0f;
  private const float _spawnRangeZ = 11.0f;

  private void Update() {
    if (GameObject.FindWithTag("Enemy") == null) {
      Debug.Log("Wave " + _wave);
      SpawnRandomEnemies(_wave++);
      if (!PowerupFound()) {
        int i = Random.Range(0, powerupPrefabs.Length);
        Vector3 spawnPoint = new(Random.Range(-_spawnRangeX, _spawnRangeX),
                                 powerupPrefabs[i].transform.position.y,
                                 Random.Range(-_spawnRangeZ, _spawnRangeZ));

        Instantiate(powerupPrefabs[i], spawnPoint,
                    powerupPrefabs[i].transform.rotation);
      }
    }
  }

  private void SpawnRandomEnemies(int numEnemies) {
    Vector3 spawnPoint;

    for (int i = numEnemies; i > 0; --i) {
      spawnPoint = new(Random.Range(-_spawnRangeX, _spawnRangeX),
                       Random.Range(_minY, _maxY),
                       Random.Range(-_spawnRangeZ, _spawnRangeZ));
      if (i % 10 == 0 && numEnemies % 5 == 0) {
        Instantiate(fastBossPrefab, spawnPoint,
          fastBossPrefab.transform.rotation);
      } else if (i % 5 == 0 && numEnemies % 5 == 0) {
        Instantiate(bossPrefab, spawnPoint, bossPrefab.transform.rotation);
      } else if (i % 3 == 0) {
        Instantiate(fastEnemyPrefab, spawnPoint,
          fastEnemyPrefab.transform.rotation);
      } else {
        Instantiate(enemyPrefab, spawnPoint, enemyPrefab.transform.rotation);
      }
    }
  }

  private bool PowerupFound() {
    return GameObject.FindWithTag("Lightning") ||
      GameObject.FindWithTag("Gem") ||
      GameObject.FindWithTag("Star") ||
      GameObject.FindWithTag("Multiplier") ||
      GameObject.FindWithTag("Blast");
  }
}
