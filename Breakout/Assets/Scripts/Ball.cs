using UnityEngine;

public class Ball : MonoBehaviour {
  private Rigidbody _rb;
  private const float _maxSpeed = 3.0f;

  private void Start() {
    _rb = GetComponent<Rigidbody>();
  }

  private void OnCollisionExit(Collision other) {
    Vector3 velocity = _rb.velocity;

    // after any collision, accelerate a bit
    velocity += velocity.normalized * 0.01f;

    // add vertical velocity when needed to avoid getting stuck
    if (Vector3.Dot(velocity.normalized, Vector3.up) < 0.1f) {
      velocity += velocity.y > 0 ? Vector3.up * 0.5f : Vector3.down * 0.5f;
    }

    if (velocity.magnitude > _maxSpeed) {
      velocity = velocity.normalized * _maxSpeed;
    }
    _rb.velocity = velocity;
  }
}
