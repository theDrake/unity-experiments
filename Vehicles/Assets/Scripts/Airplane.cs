using UnityEngine;

// INHERITANCE
public class Airplane : Vehicle {
  [SerializeField] protected Transform _propeller;
  protected float _forwardSpeed = 25.0f;
  protected float _rotationSpeed = 150.0f;
  protected float _propellerSpeed = 30.0f;
  protected const float _yMin = 1.0f;

  protected virtual void Update() {
    _propeller.Rotate(0, 0, _propellerSpeed);
  }

  // POLYMORPHISM
  public override void Move(float verticalInput, float horizontalInput) {
    if (Health <= 0) {
      return;
    }

    if (transform.position.y > _yMin &&
        !CarnageManager.Instance.OutOfBounds(transform.position)) {
      transform.Translate(_forwardSpeed * Time.deltaTime * Vector3.forward);
    }
    transform.Rotate(_rotationSpeed * verticalInput * Time.deltaTime *
                     Vector3.right);
    transform.Rotate(_rotationSpeed * -horizontalInput * Time.deltaTime *
                     Vector3.forward);
  }

  public override void MoveToward(Vector3 target) {
    if (Health <= 0) {
      return;
    }

    // Vector3 direction = (target - transform.position).normalized;
    Vector3 relativePosition = target - transform.position;
    Quaternion rotation = Quaternion.LookRotation(relativePosition, Vector3.up);
    transform.rotation = Quaternion.Lerp(transform.rotation, rotation,
                                         _rotationSpeed * Time.deltaTime);

    Move(0, 0); // no need for input
  }

  public override float GetSpeed() {
    if (Health <= 0) {
      return base.GetSpeed();
    }

    return _forwardSpeed;
  }

  protected override void Explode() {
    _rigidBody.useGravity = true;
    _propellerSpeed /= 10;
    base.Explode();
  }
}
