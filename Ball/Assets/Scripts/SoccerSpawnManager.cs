using UnityEngine;

public class SoccerSpawnManager : MonoBehaviour {
  public GameObject player, enemyPrefab, powerupPrefab;
  public static int enemyCount;
  public static int wave = 1;

  private const float spawnRangeX = 10.0f;
  private const float spawnZMin = 15.0f;
  private const float spawnZMax = 25.0f;

  private void Update() {
    // enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
    if (enemyCount == 0) {
      SpawnEnemies();
    }
  }

  private void SpawnEnemies() {
    Vector3 powerupSpawnOffset = new(0, -1.0f, -15.0f); // player's side

    enemyCount = wave;
    if (GameObject.FindGameObjectsWithTag("Powerup").Length == 0) {
      Instantiate(powerupPrefab, GetSpawnPoint() + powerupSpawnOffset,
                  powerupPrefab.transform.rotation);
    }
    for (int i = 0; i < enemyCount; ++i) {
      Instantiate(enemyPrefab, GetSpawnPoint(), enemyPrefab.transform.rotation);
    }
    SoccerEnemy[] enemies = FindObjectsByType<SoccerEnemy>(
        FindObjectsSortMode.None);
    foreach (SoccerEnemy e in enemies) {
      e.BoostSpeed(wave);
    }
    ++wave;
    ResetPlayerPosition();
  }

  private void ResetPlayerPosition () {
    player.transform.position = new(0, 1.0f, -7.0f);
    player.GetComponent<Rigidbody>().Sleep();
    // player.GetComponent<Rigidbody>().velocity = Vector3.zero;
    // player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
  }

  private Vector3 GetSpawnPoint () {
    return new(Random.Range(-spawnRangeX, spawnRangeX),
               1.0f,
               Random.Range(spawnZMin, spawnZMax));
  }
}
