using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour {
  // ENCAPSULATION
  [SerializeField] protected List<Axle> _axleList;
  protected float _maxMotorTorque = 1000.0f;
  protected float _maxSteeringAngle = 45.0f;
  protected float _health;
  protected Rigidbody _rigidBody;

  // ABSTRACTION
  public virtual void Move(float verticalInput, float horizontalInput) {
    if (_health < 0) {
      return;
    }

    float motorTorque = _maxMotorTorque * verticalInput;
    float steerAngle = _maxSteeringAngle * horizontalInput;

    foreach (Axle axle in _axleList) {
      if (axle.ProvidesSteering) {
        axle.LeftWheel.steerAngle = steerAngle;
        axle.RightWheel.steerAngle = steerAngle;
      }
      if (axle.AttachedToMotor) {
        axle.LeftWheel.motorTorque = motorTorque;
        axle.RightWheel.motorTorque = motorTorque;
      }
      ApplyLocalPositionToVisualWheel(axle.LeftWheel);
      ApplyLocalPositionToVisualWheel(axle.RightWheel);
    }
  }

  public virtual void MoveToward(Vector3 target) {
    if (_health < 0) {
      return;
    }

    float verticalInput = 1.0f;
    float horizontalInput =
        transform.InverseTransformPoint(target).normalized.x;

    Move(verticalInput, horizontalInput);
  }

  public virtual float GetSpeed() {
    return _rigidBody.velocity.magnitude;
  }

  public virtual float GetHealth() {
    return _health;
  }

  protected virtual void Start() {
    _rigidBody = GetComponent<Rigidbody>();
    _health = _rigidBody.mass;
  }

  protected virtual void FixedUpdate() {
    if (CarnageManager.OutOfBounds(transform.position)) {
      _rigidBody.Sleep();
      transform.position = Vector3.Lerp(transform.position,
                                        CarnageManager.GetCenterPoint(),
                                        0.0001f);
    }
  }

  protected virtual void OnCollisionEnter(Collision collision) {
    if (_health < 0 || collision.collider.CompareTag("Harmless")) {
      return;
    }
    _health -= collision.impulse.magnitude / 10;
    if (_health < 0) {
      Explode();
    }
  }

  protected virtual void ApplyLocalPositionToVisualWheel(WheelCollider collider) {
    if (collider.transform.childCount == 0) {
      return;
    }
    Vector3 position;
    Quaternion rotation;
    Transform visualWheel = collider.transform.GetChild(0);

    collider.GetWorldPose(out position, out rotation);
    visualWheel.transform.position = position;
    visualWheel.transform.rotation = rotation;
  }

  protected virtual void Explode() {
    if (GetComponent<Player>()) {
      Debug.Log("You lose!");
    }
  }
}

[System.Serializable]
public class Axle {
  public WheelCollider LeftWheel;
  public WheelCollider RightWheel;
  public bool AttachedToMotor;
  public bool ProvidesSteering;
}
