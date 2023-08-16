using UnityEngine;

public class Ball : MonoBehaviour {
  private Rigidbody _rigidbody;

  void Start() {
    _rigidbody = GetComponent<Rigidbody>();
  }

  private void OnCollisionExit(Collision other) {
    Vector3 velocity = _rigidbody.velocity;

    // after a collision we accelerate a bit
    velocity += velocity.normalized * 0.01f;

    // if we're not going totally vertically, this can lead to being stuck
    if (Vector3.Dot(velocity.normalized, Vector3.up) < 0.1f) {
      velocity += velocity.y > 0 ? Vector3.up * 0.5f : Vector3.down * 0.5f;
    }

    // max velocity
    if (velocity.magnitude > 3.0f) {
      velocity = velocity.normalized * 3.0f;
    }

    _rigidbody.velocity = velocity;
  }
}
