using UnityEngine;

public class Animal : MonoBehaviour {
  private const float _speedMin = 2.0f;
  private const float _speedMax = 6.0f;
  private const float _minZ = -15.0f;

  private float _speed;

  private void Start() {
    _speed = Random.Range(_speedMin, _speedMax);
    transform.Rotate(Vector3.up, 180);
  }

  private void Update() {
    if (transform.position.z > _minZ) {
      transform.Translate(_speed * Time.deltaTime * Vector3.forward);
    } else {
      Debug.Log("Game Over");
      Destroy(gameObject);
    }
  }
}
