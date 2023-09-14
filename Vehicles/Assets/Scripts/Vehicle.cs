using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vehicle : MonoBehaviour {
  public enum VehicleType {
    Car,
    Truck,
    Van,
    Bus,
    Tank,
    Airplane,
    NumVehicleTypes
  }

  [System.Serializable] protected class Axle {
    public WheelCollider LeftWheel;
    public WheelCollider RightWheel;
    public bool AttachedToMotor;
    public bool ProvidesSteering;
  }

  public VehicleType Type;
  public Vector3 FirstPersonCameraOffset { get; protected set; }
  public float Health { get; protected set; }

  [SerializeField] protected List<Axle> _axleList;
  [SerializeField] protected ParticleSystem _explosion;
  protected Rigidbody _rigidBody;
  protected Slider _healthBar;
  protected float _maxMotorTorque = 1200.0f;
  protected float _maxSteeringAngle = 45.0f;
  protected float _maxHealth;

  protected virtual void Start() {
    _rigidBody = GetComponent<Rigidbody>();
    _healthBar = GetComponentInChildren<Slider>();
    _maxHealth = _rigidBody.mass * 20.0f;
    Health = _maxHealth;
    UpdateHealth();
    FirstPersonCameraOffset = new(_healthBar.transform.localPosition.x,
                                  _healthBar.transform.localPosition.y - 1.25f,
                                  _healthBar.transform.localPosition.z);
  }

  protected virtual void FixedUpdate() {
    if (GameManager.Instance.OutOfBounds(transform.position)) {
      _rigidBody.Sleep();
      transform.position = Vector3.Lerp(transform.position,
                                        GameManager.Instance.Center, 0.0001f);
    }
  }

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

  public virtual void SetHealthBarRotation(Quaternion rotation) {
    _healthBar.transform.rotation = rotation;
  }

  protected virtual void OnCollisionEnter(Collision c) {
    if (Health > 0) {
      UpdateHealth(-c.impulse.magnitude);
    }
  }

  protected virtual void UpdateVisualWheel(WheelCollider c) {
    if (c.transform.childCount > 0) {
      c.GetWorldPose(out Vector3 position, out Quaternion rotation);
      c.transform.GetChild(0).SetPositionAndRotation(position, rotation);
    }
  }

  protected virtual void UpdateHealth(float adjustment=0) {
    Health += adjustment;
    if (Health > 0) {
      _healthBar.value = Health / _maxHealth;
    } else {
      Explode();
    }
  }

  protected virtual void Explode() {
    // _healthBar.gameObject.SetActive(false);
    if (GetComponent<Player>()) {
      CarnageCanvas.ShowGameOver();
    } else {
      CarnageCanvas.UpdateNumEnemies(-1);
    }
    Instantiate<ParticleSystem>(_explosion, transform.position,
                                transform.rotation);
    gameObject.SetActive(false);
  }
}
