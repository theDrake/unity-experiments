using UnityEngine;

public class BalloonMoveLeft : MonoBehaviour {
  private const float _speed = 10.0f;
  private const float _leftBound = -10.0f;

  private BalloonPlayer _player;

  private void Start() {
    _player = GameObject.FindWithTag("Player").GetComponent<BalloonPlayer>();
  }

  private void Update() {
    if (!_player.gameOver) {
      transform.Translate(_speed * Time.deltaTime * Vector3.left, Space.World);
    }
    if (transform.position.x < _leftBound &&
        !gameObject.CompareTag("Background")) {
      Destroy(gameObject);
    }
  }
}
