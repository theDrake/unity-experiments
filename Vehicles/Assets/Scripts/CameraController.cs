using UnityEngine;

public class CameraController : MonoBehaviour {
  private Vehicle _focalObject;
  private bool _1stPerson = false;
  private Vector3 _positionOffset;
  private Vector3 _3rdPersonOffset;
  private static readonly Vector3 _3rdPersonOffsetMin = new(0, 7.0f, -12.0f);
  private static readonly Vector3 _3rdPersonOffsetIncrement = new(0, 0, -4.0f);
  private const float _3rdPersonMaxDistance = 50.0f;
  private Vector3 _rotationOffset = new(0, 0, 0);

  private void Start() {
    _focalObject = FindAnyObjectByType<Player>().GetComponent<Vehicle>();
    _positionOffset = _3rdPersonOffset = _3rdPersonOffsetMin;
  }

  private void LateUpdate() {
    UpdatePositionOffset();
    transform.SetPositionAndRotation(_focalObject.transform.position +
        _focalObject.transform.rotation * _positionOffset,
        _focalObject.transform.rotation * Quaternion.Euler(_rotationOffset));
  }

  private void UpdatePositionOffset() {
    if (Input.GetKeyUp(KeyCode.Tab)) {
      _1stPerson = !_1stPerson;
    } else {
      if (Input.mouseScrollDelta.y > 0) {
        _3rdPersonOffset -= _3rdPersonOffsetIncrement;
        if (_3rdPersonOffset.magnitude < _3rdPersonOffsetMin.magnitude) {
          _1stPerson = true;
          _3rdPersonOffset = _3rdPersonOffsetMin;
        }
      } else if (Input.mouseScrollDelta.y < 0) {
        if (_1stPerson) {
          _1stPerson = false;
          _3rdPersonOffset = _3rdPersonOffsetMin;
        } else if (_3rdPersonOffset.magnitude < _3rdPersonMaxDistance) {
          _3rdPersonOffset += _3rdPersonOffsetIncrement;
        }
      }
    }
    if (_1stPerson) {
      _positionOffset = _focalObject.FirstPersonCameraOffset;
    } else {
      _positionOffset = _3rdPersonOffset;
    }
  }
}
