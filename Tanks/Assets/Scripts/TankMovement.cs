using UnityEngine;

public class TankMovement : MonoBehaviour {
  public int PlayerNumber = 1;
  public AudioSource MovementAudio;
  public AudioClip EngineIdling;
  public AudioClip EngineDriving;

  private Rigidbody _rb;
  private const float _speed = 12.0f;
  private const float _turnSpeed = 180.0f;
  private const float _pitchRange = 0.2f;
  private float _movementInput;
  private float _turnInput;
  private float _originalPitch;
  private string _movementAxis;
  private string _turnAxis;

  private void Awake() {
    _rb = GetComponent<Rigidbody>();
  }

  private void OnEnable() {
    _rb.isKinematic = false;
    _movementInput = 0f;
    _turnInput = 0f;
  }

  private void Start() {
    _movementAxis = "Vertical" + PlayerNumber;
    _turnAxis = "Horizontal" + PlayerNumber;
    _originalPitch = MovementAudio.pitch;
  }

  private void FixedUpdate() {
    Move();
    Turn();
  }

  private void Update() {
    _movementInput = Input.GetAxis(_movementAxis);
    _turnInput = Input.GetAxis(_turnAxis);
    EngineAudio();
  }

  private void OnDisable() {
    _rb.isKinematic = true;
  }

  private void Move() {
    Vector3 movement = _movementInput * _speed * Time.deltaTime *
        transform.forward;

    _rb.MovePosition(_rb.position + movement);
  }

  private void Turn() {
    float turn = _turnInput * _turnSpeed * Time.deltaTime;
    Quaternion turnRotation = Quaternion.Euler(0, turn, 0);

    _rb.MoveRotation(_rb.rotation * turnRotation);
  }

  private void EngineAudio() {
    if (Mathf.Abs(_movementInput) < 0.1f && Mathf.Abs(_turnInput) < 1.0f) {
      if (MovementAudio.clip == EngineDriving) {
        MovementAudio.clip = EngineIdling;
        MovementAudio.pitch = Random.Range(_originalPitch - _pitchRange,
                                           _originalPitch + _pitchRange);
        MovementAudio.Play();
      }
    } else if (MovementAudio.clip == EngineIdling) {
      MovementAudio.clip = EngineDriving;
      MovementAudio.pitch = Random.Range(_originalPitch - _pitchRange,
                                         _originalPitch + _pitchRange);
      MovementAudio.Play();
    }
  }
}
