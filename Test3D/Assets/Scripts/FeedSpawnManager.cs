using UnityEngine;

public class FeedSpawnManager : MonoBehaviour {
  private const float _xRange = 20.0f;
  private const float _zRange = 15.0f;

  public GameObject[] animals;

  private void Update() {
    if (Random.Range(0, 100) == 0) {
      SpawnAnimal(Random.Range(0, animals.Length));
    }
  }

  private void SpawnAnimal(int animalIndex) {
    Instantiate(animals[animalIndex],
                new(Random.Range(-_xRange, _xRange), 0, _zRange),
                animals[animalIndex].transform.rotation);
  }
}
