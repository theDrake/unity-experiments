using UnityEngine;

// INHERITANCE
public class Player : GameCharacter {
  private void FixedUpdate() {
    if (Alive()) {
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
