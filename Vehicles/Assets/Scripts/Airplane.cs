using UnityEngine;

public class Airplane : Vehicle {
  protected const float _yMin = 1.0f;

  [SerializeField] protected Transform _propeller;
  protected float _forwardSpeed = 28.0f;
  protected float _rotationSpeed = 140.0f;
  protected float _propellerSpeed = 31.0f;

  protected virtual void Update() {
    _propeller.Rotate(0, 0, _propellerSpeed);
  }

  public override void Move(float verticalInput, float horizontalInput) {
    if (Health <= 0) {
      return;
    }
    if (transform.position.y > _yMin &&
        !GameManager.Instance.OutOfBounds(transform.position)) {
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
    transform.rotation = Quaternion.Lerp(transform.rotation,
        Quaternion.LookRotation(target - transform.position, Vector3.up),
        Time.deltaTime);
    Move(0, 0); // no additional rotation, just move forward
  }

  public override float GetSpeed() {
    if (Health <= 0) {
      return base.GetSpeed();
    }

    return _forwardSpeed;
  }

  // protected override void Explode() {
  //   _rigidBody.useGravity = true;
  //   _propellerSpeed /= 10;
  //   base.Explode();
  // }
}
