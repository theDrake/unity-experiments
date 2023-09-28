using UnityEngine;

public class FetchPlayer : MonoBehaviour {
  private const float _spawnInterval = 0.5f;

  public GameObject dog;

  private float _timeOfLastSpawn;

  private void Update() {
    if (Input.GetKeyDown(KeyCode.Space) &&
        Time.fixedTime - _timeOfLastSpawn > _spawnInterval) {
      Instantiate(dog, transform.position, dog.transform.rotation);
      _timeOfLastSpawn = Time.fixedTime;
    }
  }
}
