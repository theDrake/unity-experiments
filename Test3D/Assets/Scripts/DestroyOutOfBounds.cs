using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour {
  private const float _minX = -30.0f;
  private const float _minY = -5.0f;

  private void Update() {
    if (transform.position.x < _minX || transform.position.y < _minY) {
      Destroy(gameObject);
    }
  }
}
