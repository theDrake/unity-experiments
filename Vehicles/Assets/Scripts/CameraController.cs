using UnityEngine;

public class CameraController : MonoBehaviour {
  private static readonly Vector3 _3rdPersonOffsetMin = new(0, 7.0f, -12.0f);
  private static readonly Vector3 _3rdPersonOffsetIncrement = new(0, 0, -4.0f);
  private const float _3rdPersonMaxDistance = 50.0f;

  private Vehicle _focalObject;
  private Vector3 _positionOffset;
  private Vector3 _3rdPersonOffset;
  private Vector3 _rotationOffset = new(0, 0, 0);
  private bool _1stPerson = false;

  private void Start() {
    _positionOffset = _3rdPersonOffset = _3rdPersonOffsetMin;
  }

  private void LateUpdate() {
    if (_focalObject) {
      if (Input.GetKeyUp(KeyCode.Tab)) {
        _1stPerson = !_1stPerson;
        UpdatePositionOffset();
      } else if (Input.mouseScrollDelta.y > 0) {
        ZoomIn();
      } else if (Input.mouseScrollDelta.y < 0) {
        ZoomOut();
      }
      transform.SetPositionAndRotation(_focalObject.transform.position +
          _focalObject.transform.rotation * _positionOffset,
          _focalObject.transform.rotation * Quaternion.Euler(_rotationOffset));
    } else {
      _focalObject = FindAnyObjectByType<Player>().GetComponent<Vehicle>();
    }
  }

  private void UpdatePositionOffset() {
    if (_1stPerson) {
      _positionOffset = _focalObject.FirstPersonCameraOffset;
    } else {
      _positionOffset = _3rdPersonOffset;
    }
  }

  private void ZoomIn() {
    _3rdPersonOffset -= _3rdPersonOffsetIncrement;
    if (_3rdPersonOffset.magnitude < _3rdPersonOffsetMin.magnitude) {
      _1stPerson = true;
      _3rdPersonOffset = _3rdPersonOffsetMin;
    }
    UpdatePositionOffset();
  }

  private void ZoomOut() {
    if (_1stPerson) {
      _1stPerson = false;
      _3rdPersonOffset = _3rdPersonOffsetMin;
    } else if (_3rdPersonOffset.magnitude < _3rdPersonMaxDistance) {
      _3rdPersonOffset += _3rdPersonOffsetIncrement;
    }
    UpdatePositionOffset();
  }
}
