using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Tank : MonoBehaviour {
  [HideInInspector] public string PlayerText;
  [HideInInspector] public int PlayerNum {
    get { return _playerNum; }
    set {
      _playerNum = value;
      PlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(_color) +
          ">PLAYER " + _playerNum + "</color>";
    }
  }
  [HideInInspector] public int Wins;
  public Slider HealthSlider;
  public Image HealthFillImage;
  public GameObject ExplosionPrefab;
  public AudioSource MovementAudio;
  public AudioClip EngineIdling;
  public AudioClip EngineDriving;
  public Rigidbody Shell;
  public Transform FireTransform;
  public Slider AimSlider;
  public AudioSource ShootingAudio;
  public AudioClip ChargingClip;
  public AudioClip FireClip;

  private GameObject _canvas;
  private Rigidbody _rb;
  private Color _color;
  private AudioSource _explosionAudio;
  private ParticleSystem _explosion;
  private readonly Color _fullHealthColor = Color.green;
  private readonly Color _zeroHealthColor = Color.red;
  private Vector3 _startingPosition;
  private Quaternion _startingRotation;
  private const float _startingHealth = 100.0f;
  private float _health;
  private bool _dead;
  private bool _controlEnabled;
  private const float _speed = 12.0f;
  private const float _turnSpeed = 180.0f;
  private const float _pitchRange = 0.2f;
  private float _movementInput;
  private float _turnInput;
  private float _originalPitch;
  private string _movementAxis;
  private string _turnAxis;
  private const float _minLaunchForce = 15.0f;
  private const float _maxLaunchForce = 30.0f;
  private const float _maxChargeTime = 0.75f;
  private float _currentLaunchForce;
  private float _chargeSpeed;
  private string _fireButton;
  private bool _fired;
  private int _playerNum;

  private void Awake() {
    _color = Random.ColorHSV();
    _rb = GetComponent<Rigidbody>();
    _canvas = GetComponentInChildren<Canvas>().gameObject;
    _explosion = Instantiate(ExplosionPrefab).GetComponent<ParticleSystem>();
    _explosionAudio = _explosion.GetComponent<AudioSource>();
    _explosion.gameObject.SetActive(false);
    _startingPosition = transform.position;
    _startingRotation = transform.rotation;
  }

  private void OnEnable() {
    _rb.isKinematic = false;
    _controlEnabled = false;
    _movementInput = _turnInput = 0;
    _currentLaunchForce = AimSlider.value = _minLaunchForce;
    _health = _startingHealth;
    _dead = false;
    SetHealthUI();
  }

  private void Start() {
    foreach (MeshRenderer r in GetComponentsInChildren<MeshRenderer>()) {
      r.material.color = _color;
    }
    _movementAxis = "Vertical" + PlayerNum;
    _turnAxis = "Horizontal" + PlayerNum;
    _originalPitch = MovementAudio.pitch;
    _fireButton = "Fire" + PlayerNum;
    _chargeSpeed = (_maxLaunchForce - _minLaunchForce) / _maxChargeTime;
  }

  private void FixedUpdate() {
    if (_controlEnabled) {
      Move();
      Turn();
    }
  }

  private void Update() {
    if (!_controlEnabled) {
      return;
    }
    _movementInput = Input.GetAxis(_movementAxis);
    _turnInput = Input.GetAxis(_turnAxis);
    EngineAudio();
    AimSlider.value = _minLaunchForce;
    if (Input.GetButtonDown(_fireButton)) {
      _fired = false;
      _currentLaunchForce = _minLaunchForce;
      ShootingAudio.clip = ChargingClip;
      ShootingAudio.Play();
    } else if (Input.GetButton(_fireButton) && !_fired) {
      _currentLaunchForce += _chargeSpeed * Time.deltaTime;
      AimSlider.value = _currentLaunchForce;
    } else if (Input.GetButtonUp(_fireButton) && !_fired) {
      Fire();
    } else if (_currentLaunchForce >= _maxLaunchForce && !_fired) {
      _currentLaunchForce = _maxLaunchForce;
      Fire();
    }
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

  private void Fire() {
    Rigidbody shell = Instantiate(Shell, FireTransform.position,
                                  FireTransform.rotation) as Rigidbody;

    _fired = true;
    shell.velocity = _currentLaunchForce * FireTransform.forward;
    ShootingAudio.clip = FireClip;
    ShootingAudio.Play();
    _currentLaunchForce = _minLaunchForce;
  }

  public void TakeDamage(float amount) {
    _health -= amount;
    SetHealthUI();
    if (_health <= 0 && !_dead) {
      Explode();
    }
  }

  private void SetHealthUI() {
    HealthSlider.value = _health;
    HealthFillImage.color = Color.Lerp(_zeroHealthColor, _fullHealthColor,
                                       _health / _startingHealth);
  }

  private void Explode() {
    _dead = true;
    _explosion.transform.position = transform.position;
    _explosion.gameObject.SetActive(true);
    _explosion.Play();
    _explosionAudio.Play();
    gameObject.SetActive(false);
  }

  public void DisableControl() {
    _controlEnabled = false;
    _canvas.SetActive(false);
  }

  public void EnableControl() {
    _controlEnabled = true;
    _canvas.SetActive(true);
  }

  public void Reset() {
    transform.SetPositionAndRotation(_startingPosition, _startingRotation);
    gameObject.SetActive(false);
    gameObject.SetActive(true);
  }
}
