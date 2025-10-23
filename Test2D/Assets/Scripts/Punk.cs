using UnityEngine;

public class Punk : MonoBehaviour {
  private const float _minY = -3.0f;
  private const float _spawnMinX = 0.0f;
  private const float _spawnMaxX = 4.0f;
  private const float _spawnMinY = 2.0f;
  private const float _spawnMaxY = 3.0f;

  private Rigidbody2D _rb;

  private void Start() {
    _rb = GetComponent<Rigidbody2D>();
  }

  private void Update() {
    if (transform.position.y < _minY) {
      _rb.linearVelocity = Vector2.zero;
      transform.SetPositionAndRotation(new(Random.Range(_spawnMinX,
                                                        _spawnMaxX),
                                           Random.Range(_spawnMinY,
                                                        _spawnMaxY)),
                                       transform.rotation);
    }
  }
}
