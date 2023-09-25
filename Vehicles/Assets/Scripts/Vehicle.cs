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

  protected const float _maxMotorTorque = 1500.0f;
  protected const float _maxSteeringAngle = 45.0f;
  protected const float _envDamageMultiplier = -1000.0f;

  public VehicleType Type;
  public Vector3 FirstPersonCameraOffset { get; protected set; }
  public float Health { get; protected set; }
  public bool IsPlayer { get; private set; }

  [SerializeField] protected List<Axle> _axleList;
  [SerializeField] protected ParticleSystem _explosion;
  protected Rigidbody _rigidBody;
  protected Slider _healthBar;
  protected float _maxHealth;
  protected bool _dead = false;

  protected virtual void Awake() {
    _rigidBody = GetComponent<Rigidbody>();
    _healthBar = GetComponentInChildren<Slider>();
    _maxHealth = _rigidBody.mass * 20.0f;
    Health = _maxHealth;
    UpdateHealth();
    FirstPersonCameraOffset = new(_healthBar.transform.localPosition.x,
                                  _healthBar.transform.localPosition.y - 1.25f,
                                  _healthBar.transform.localPosition.z);
  }

  protected virtual void Start() {
    if (GetComponent<Player>()) {
      IsPlayer = true;
    }
  }

  protected virtual void FixedUpdate() {
    if (GameManager.Instance.OutOfBounds(transform.position)) {
      _rigidBody.Sleep();
      transform.position = Vector3.Lerp(transform.position,
                                        GameManager.Instance.Center, 0.0001f);
      UpdateHealth(Time.deltaTime * _envDamageMultiplier);
    }
  }

  protected virtual void LateUpdate() {
    _healthBar.transform.rotation =
        CameraController.Instance.transform.rotation;
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
      UpdateVisualWheel(axle.LeftWheel);
      UpdateVisualWheel(axle.RightWheel);
    }
  }

  public virtual void MoveToward(Vector3 target) {
    float verticalInput = 1.0f;
    float horizontalInput =
        transform.InverseTransformPoint(target).normalized.x;

    Move(verticalInput, horizontalInput);
  }

  public virtual float GetSpeed() {
    return _rigidBody.velocity.magnitude;
  }

  protected virtual void OnCollisionEnter(Collision c) {
    if (_dead) {
      return;
    }
    if (IsPlayer) {
      CarnageCanvas.Score += (int) c.impulse.magnitude / 100;
    }
    UpdateHealth(-c.impulse.magnitude);
  }

  protected virtual void UpdateVisualWheel(WheelCollider c) {
    if (c.transform.childCount > 0) {
      c.GetWorldPose(out Vector3 position, out Quaternion rotation);
      c.transform.GetChild(0).SetPositionAndRotation(position, rotation);
    }
  }

  protected virtual void UpdateHealth(float adjustment=0) {
    if (_dead) {
      return;
    }
    Health += adjustment;
    if (Health > 0) {
      _healthBar.value = Health / _maxHealth;
    } else {
      Explode();
    }
  }

  protected virtual void Explode() {
    if (_dead) {
      return;
    }
    _dead = true;
    Instantiate<ParticleSystem>(_explosion, transform.position,
                                transform.rotation);
    if (IsPlayer) {
      CarnageCanvas.ShowGameOver();
      CameraController.Instance.FocalObject = null;
    } else {
      CarnageCanvas.UpdateNumEnemies(-1);
    }
    gameObject.SetActive(false);
  }
}
