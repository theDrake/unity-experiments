using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vehicle : MonoBehaviour {
  // ENCAPSULATION
  public enum VehicleType {
    Car,
    Truck,
    Van,
    Bus,
    Tank,
    Airplane,
    NumVehicleTypes
  }
  public VehicleType Type;
  public float Health { get; protected set; }
  public Vector3 FirstPersonCameraOffset { get; protected set; }

  [System.Serializable] protected class Axle {
    public WheelCollider LeftWheel;
    public WheelCollider RightWheel;
    public bool AttachedToMotor;
    public bool ProvidesSteering;
  }
  [SerializeField] protected List<Axle> _axleList;
  protected float _maxMotorTorque = 1200.0f;
  protected float _maxSteeringAngle = 45.0f;
  protected Rigidbody _rigidBody;
  protected Slider _healthBar;

  protected virtual void Start() {
    _rigidBody = GetComponent<Rigidbody>();
    _healthBar = GetComponentInChildren<Slider>();
    Health = _rigidBody.mass;
    UpdateHealthBar();
    FirstPersonCameraOffset = new Vector3(
        _healthBar.transform.localPosition.x,
        _healthBar.transform.localPosition.y - 1.25f,
        _healthBar.transform.localPosition.z
    );
  }

  protected virtual void FixedUpdate() {
    if (CarnageManager.Instance.OutOfBounds(transform.position)) {
      _rigidBody.Sleep();
      transform.position = Vector3.Lerp(transform.position,
                                        CarnageManager.Instance.CenterPoint,
                                        0.0001f);
    }
  }

  // ABSTRACTION
  public virtual void Move(float verticalInput, float horizontalInput) {
    if (Health <= 0) {
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
      UpdateVisualWheel(axle.LeftWheel);
      UpdateVisualWheel(axle.RightWheel);
    }
  }

  public virtual void MoveToward(Vector3 target) {
    if (Health <= 0) {
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

  protected virtual void OnCollisionEnter(Collision collision) {
    if (Health <= 0 || collision.collider.CompareTag("Harmless")) {
      return;
    }
    Health -= collision.impulse.magnitude / 10;
    UpdateHealthBar();
    if (Health <= 0) {
      Explode();
    }
  }

  protected virtual void UpdateVisualWheel(WheelCollider collider) {
    if (collider.transform.childCount == 0) {
      return;
    }
    collider.GetWorldPose(out Vector3 position, out Quaternion rotation);
    collider.transform.GetChild(0).SetPositionAndRotation(position, rotation);
  }

  protected virtual void UpdateHealthBar() {
    _healthBar.value = Health / _rigidBody.mass;
  }

  public virtual void SetHealthBarRotation(Quaternion rotation) {
    _healthBar.transform.rotation = rotation;
  }

  protected virtual void Explode() {
    _healthBar.gameObject.SetActive(false);
    if (GetComponent<Player>()) {
      Debug.Log("You lose!");
    } else {
      CarnageManager.Instance.CheckForVictory();
    }
  }
}
