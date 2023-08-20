using UnityEngine;

public class CameraController : MonoBehaviour {
  // ENCAPSULATION
  private Vehicle _focalObject;
  private bool _firstPerson = false;
  private Vector3 _positionOffset;
  private Vector3 _positionOffset1st = new Vector3(0f, 2.25f, 3.75f);
  private Vector3 _positionOffset3rd;
  private Vector3 _positionOffset3rdMin = new Vector3(0, 7, -14);
  private Vector3 _positionOffset3rdMax = new Vector3(0, 21, -42);
  private Vector3 _positionOffset3rdModifier = new Vector3(0, 2, -4);
  private Vector3 _rotationOffset = new Vector3(0, 0, 0);

  void Start() {
    _focalObject = FindObjectOfType<Player>().GetComponent<Vehicle>();
    _positionOffset = _positionOffset3rd = _positionOffset3rdMin;
  }

  void LateUpdate() {
    UpdatePositionOffset();
    transform.position = _focalObject.transform.position +
        _focalObject.transform.rotation * _positionOffset;
    transform.rotation = _focalObject.transform.rotation *
        Quaternion.Euler(_rotationOffset);
  }

  // ABSTRACTION
  void UpdatePositionOffset() {
    if (Input.GetKeyUp(KeyCode.Tab)) {
      _firstPerson = !_firstPerson;
    } else {
      if (Input.mouseScrollDelta.y > 0) {
        _positionOffset3rd -= _positionOffset3rdModifier;
        if (_positionOffset3rd.y < _positionOffset3rdMin.y) {
          _positionOffset3rd = _positionOffset3rdMin;
          _firstPerson = true;
        }
      } else if (Input.mouseScrollDelta.y < 0) {
        if (_firstPerson) {
          _firstPerson = false;
        } else {
          _positionOffset3rd += _positionOffset3rdModifier;
        }
        if (_positionOffset3rd.y > _positionOffset3rdMax.y) {
          _positionOffset3rd = _positionOffset3rdMax;
        }
      }
    }
    if (_firstPerson) {
      _positionOffset = _positionOffset1st;
    } else {
      _positionOffset = _positionOffset3rd;
    }
  }
}
