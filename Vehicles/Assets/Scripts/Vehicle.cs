using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour {
  // ENCAPSULATION
  [SerializeField] protected List<Axle> _axleList;
  protected float _maxMotorTorque = 800;
  protected float _maxSteeringAngle = 30;
  protected float _envRadius = 250;
  protected Vector3 _center = new Vector3(0, 2, 175);
  protected Rigidbody _rigidBody;

  public virtual void Start() {
    // transform.SetPositionAndRotation(_center, initialRotation);
    _rigidBody = GetComponent<Rigidbody>();
  }

  public virtual void Update() {
    if (Vector3.Distance(transform.position, _center) > _envRadius) {
      _rigidBody.Sleep();
      transform.position = Vector3.Lerp(transform.position, _center, 0.01f);
    }
  }

  public virtual void Move(float verticalInput, float horizontalInput) {
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

  // ABSTRACTION
  public void ApplyLocalPositionToVisualWheel(WheelCollider collider) {
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

  public float GetSpeed() {
    return _rigidBody.velocity.magnitude;
  }
}

[System.Serializable]
public class Axle {
  public WheelCollider LeftWheel;
  public WheelCollider RightWheel;
  public bool AttachedToMotor;
  public bool ProvidesSteering;
}
