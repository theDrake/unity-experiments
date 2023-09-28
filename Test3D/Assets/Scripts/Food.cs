using UnityEngine;

public class Food : MonoBehaviour {
  private const float _speed = 20.0f;
  private const float _maxZ = 12.0f;

  private void Update() {
    if (transform.position.z > _maxZ) {
      Destroy(gameObject);
    }
    transform.Translate(_speed * Time.deltaTime * Vector3.forward);
  }

  private void OnTriggerEnter(Collider other) {
    Destroy(gameObject);
    Destroy(other.gameObject);
  }
}
