using UnityEngine;

public class Player : MonoBehaviour {
  private Vehicle _vehicle;
  // private float _forwardSpeed = 80;
  // private float _turnSpeed = 60;
  // private float _forwardForce = 10000000.0f;

  void Start() {
    _vehicle = GetComponent<Vehicle>();
  }

  void FixedUpdate() {
    if (_vehicle.GetHealth() > 0) {
      _vehicle.Move(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
    }
    // transform.Translate(Vector3.forward * Time.deltaTime * _forwardSpeed *
    //   verticalInput);
    // _rigidBody.AddRelativeForce(Vector3.forward * _forwardForce *
    //                             verticalInput * Time.deltaTime);
    // transform.Rotate(Vector3.up * Time.deltaTime * _turnSpeed *
    //                  horizontalInput);
  }
}
