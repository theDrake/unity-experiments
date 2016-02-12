using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
  public float spawnTime = 5f;  // Amount of time between each spawn.
  public float spawnDelay = 3f;  // Amount of time before spawning starts.
  public GameObject[] enemies;  // Array of enemy prefabs.

  void Start() {
    InvokeRepeating("Spawn", spawnDelay, spawnTime);
  }

  void Spawn() {
    // Instantiate a random enemy.
    int enemyIndex = Random.Range(0, enemies.Length);
    Instantiate(enemies[enemyIndex], transform.position, transform.rotation);

    // Play the spawning effect from all of the particle systems.
    foreach (ParticleSystem p in GetComponentsInChildren<ParticleSystem>()) {
      p.Play();
    }
  }
}
