using UnityEngine;

// INHERITANCE
public class Airplane : Vehicle {
  protected float _forwardSpeed = 30.0f;
  protected float _rotationSpeed = 150.0f;

  // POLYMORPHISM
  public override void Move(float verticalInput, float horizontalInput) {
    transform.Translate(Vector3.forward * Time.deltaTime * _forwardSpeed);
    transform.Rotate(_rotationSpeed * verticalInput * Time.deltaTime *
                     Vector3.right);
    transform.Rotate(_rotationSpeed * -horizontalInput * Time.deltaTime *
                     Vector3.forward);
  }

  public override void MoveToward(Vector3 target) {
    // Vector3 direction = (target - transform.position).normalized;
    Vector3 relativePosition = target - transform.position;
    Quaternion rotation = Quaternion.LookRotation(relativePosition, Vector3.up);
    transform.rotation = Quaternion.Lerp(transform.rotation, rotation,
                                         Time.deltaTime * _rotationSpeed);
    Move(0, 0); // no need for input
  }
}
