using UnityEngine;

public class Paddle : MonoBehaviour {
  private const float _speed = 4.0f;
  private const float _boundary = 2.0f;

  void Update() {
    float input = Input.GetAxis("Horizontal");
    Vector3 position = transform.position;

    position.x += input * _speed * Time.deltaTime;
    if (position.x > _boundary) {
      position.x = _boundary;
    } else if (position.x < -_boundary) {
      position.x = -_boundary;
    }
    transform.position = position;
  }
}
