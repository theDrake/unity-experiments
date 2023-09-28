using UnityEngine;

public class BalloonBackground : MonoBehaviour {
  private Vector3 _startPoint;
  private float _repeatWidth;

  private void Start() {
    _startPoint = transform.position;
    _repeatWidth = GetComponent<BoxCollider>().size.x / 2;
  }

  private void Update() {
    if (transform.position.x < _startPoint.x - _repeatWidth) {
      transform.position = _startPoint;
    }
  }
}
