using UnityEngine;

public class Enemy : MonoBehaviour {
  [SerializeField] private Vehicle _vehicle;
  // private float _forwardSpeed = 20, _turnSpeed = 20;
  private GameObject _target;

  void Start() {
    _target = GameObject.Find("Player");
    if (!_vehicle) {
      _vehicle = GetComponent<Vehicle>();
    }
  }

  void FixedUpdate() {
    float verticalInput = 1.0f;
    float horizontalInput = 1.0f;

    if (_vehicle) {
      _vehicle.Move(verticalInput, horizontalInput);
    }
  }

  // void Update() {
  //   transform.Translate(Vector3.forward * Time.deltaTime * _forwardSpeed);
  //   transform.LookAt(_target.transform.position);
  // }
}
