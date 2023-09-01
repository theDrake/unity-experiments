using UnityEngine;

public class CameraControl : MonoBehaviour {
  [HideInInspector]
  public Transform[] Targets;

  private Camera _camera;
  private const float _dampTime = 0.2f;
  private const float _screenEdgeBuffer = 4.0f;
  private const float _minSize = 6.5f;
  private float _zoomSpeed;
  private Vector3 _moveVelocity;
  private Vector3 _desiredPosition;

  private void Awake() {
    _camera = GetComponentInChildren<Camera>();
  }

  private void FixedUpdate() {
    Move();
    Zoom();
  }

  private void Move() {
    FindAveragePosition();
    transform.position = Vector3.SmoothDamp(transform.position,
        _desiredPosition, ref _moveVelocity, _dampTime);
  }

  private void Zoom() {
    _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize,
        FindRequiredSize(), ref _zoomSpeed, _dampTime);
  }

  private void FindAveragePosition() {
    Vector3 averagePosition = new();
    int numTargets = 0;

    for (int i = 0; i < Targets.Length; ++i) {
      if (!Targets[i].gameObject.activeSelf) {
        continue;
      }
      averagePosition += Targets[i].position;
      ++numTargets;
    }
    if (numTargets > 0) {
      averagePosition /= numTargets;
    }
    averagePosition.y = transform.position.y;
    _desiredPosition = averagePosition;
  }

  private float FindRequiredSize() {
    Vector3 local = transform.InverseTransformPoint(_desiredPosition);
    float size = 0f;

    for (int i = 0; i < Targets.Length; ++i) {
      if (!Targets[i].gameObject.activeSelf) {
        continue;
      }
      Vector3 target = transform.InverseTransformPoint(Targets[i].position);
      Vector3 desired = target - local;

      size = Mathf.Max(size, Mathf.Abs(desired.y));
      size = Mathf.Max(size, Mathf.Abs(desired.x) / _camera.aspect);
    }
    size += _screenEdgeBuffer;
    size = Mathf.Max(size, _minSize);

    return size;
  }

  public void SetStartPositionAndSize() {
    FindAveragePosition();
    transform.position = _desiredPosition;
    _camera.orthographicSize = FindRequiredSize();
  }
}
