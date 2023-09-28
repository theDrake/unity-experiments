using UnityEngine;

public class RunEnvironment : MonoBehaviour {
  private RunPlayer _player;
  private Vector3 _startPoint;
  private float _xBound = -10.0f;

  private void Start() {
    _player = FindAnyObjectByType<RunPlayer>();
    if (gameObject.CompareTag("Background")) {
      _startPoint = transform.position;
      _xBound = _startPoint.x -
          GetComponent<SpriteRenderer>().bounds.size.x / 2;
    }
  }

  private void Update() {
    if (!_player.gameOver) {
      transform.Translate(_player.speed * Time.deltaTime * Vector3.left);
      if (transform.position.x < _xBound) {
        if (gameObject.CompareTag("Background")) {
          transform.position = _startPoint;
        } else if (gameObject.CompareTag("Obstacle")) {
          Destroy(gameObject);
        }
      }
    }
  }
}
