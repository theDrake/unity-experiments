using UnityEngine;

public class CameraController : MonoBehaviour {
  public Vehicle FocalObject {
    get { return _focalObject; }
    set {
      _focalObject = value;
      UpdateParent();
    }
  }

  public static CameraController Instance { get; private set; }

  private static readonly Vector3 _3rdPersonOffsetMin = new(0, 7.0f, -12.0f);
  private static readonly Vector3 _3rdPersonOffsetIncrement = new(0, 0, -4.0f);
  private const float _3rdPersonMaxDistance = 50.0f;
  private const float _rotationSpeed = 180.0f;

  private Camera _camera;
  private Vehicle _focalObject;
  private Vector3 _positionOffset;
  private Vector3 _3rdPersonOffset;
  private bool _1stPerson = false;

  private void Awake() {
    if (!Instance) {
      Instance = this;
      DontDestroyOnLoad(gameObject);
      _camera = GetComponentInChildren<Camera>();
      ResetCamera();
    } else {
      Destroy(gameObject);
    }
  }

  private void LateUpdate() {
    if (_focalObject) {
      if (Input.GetKeyUp(KeyCode.Tab)) {
        _1stPerson = !_1stPerson;
        UpdatePositionOffset();
      } else if (Input.mouseScrollDelta.y > 0 ||
                 Input.GetKey(KeyCode.UpArrow)) {
        ZoomIn();
      } else if (Input.mouseScrollDelta.y < 0 ||
                 Input.GetKey(KeyCode.DownArrow)) {
        ZoomOut();
      }
      // if (Input.GetKey(KeyCode.UpArrow)) {
      //   transform.Rotate(Vector3.left, _rotationSpeed * Time.deltaTime);
      // }
      // if (Input.GetKey(KeyCode.DownArrow)) {
      //   transform.Rotate(Vector3.right, _rotationSpeed * Time.deltaTime);
      // }
      if (Input.GetKey(KeyCode.LeftArrow)) {
        transform.Rotate(Vector3.down, _rotationSpeed * Time.deltaTime);
      }
      if (Input.GetKey(KeyCode.RightArrow)) {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
      }
      transform.Rotate(Vector3.up,
          Input.GetAxis("Mouse X") * _rotationSpeed * Time.deltaTime);
      // transform.Rotate(Vector3.left,
      //     Input.GetAxis("Mouse Y") * _rotationSpeed * Time.deltaTime);
    } else {
      Player player = FindAnyObjectByType<Player>();

      if (player) {
        player.TryGetComponent<Vehicle>(out _focalObject);
        UpdateParent();
      }
    }
  }

  private void ResetCamera() {
    transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    _3rdPersonOffset = _3rdPersonOffsetMin;
    UpdatePositionOffset();
  }

  private void UpdateParent() {
    if (_focalObject) {
      transform.SetParent(_focalObject.transform);
      ResetCamera();
    } else {
      transform.SetParent(null);
    }
  }

  private void UpdatePositionOffset() {
    if (_1stPerson && _focalObject) {
      _positionOffset = _focalObject.FirstPersonCameraOffset;
    } else {
      _1stPerson = false;
      _positionOffset = _3rdPersonOffset;
    }
    _camera.transform.position = transform.position + transform.rotation *
        _positionOffset;
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
