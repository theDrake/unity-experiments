using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Tank : MonoBehaviour {
  private readonly Color _fullHealthColor = Color.green;
  private readonly Color _zeroHealthColor = Color.red;
  private const float _startingHealth = 100.0f;
  private const float _speed = 12.0f;
  private const float _turnSpeed = 180.0f;
  private const float _minLaunchForce = 15.0f;
  private const float _maxLaunchForce = 30.0f;
  private const float _maxChargeTime = 0.75f;
  private const float _chargeSpeed = (_maxLaunchForce - _minLaunchForce) /
      _maxChargeTime;
  private const float _fireDelay = 0.5f; // seconds
  private const float _pitchRange = 0.2f; // for audio variation
  private const float _maxFireAngle = 0.16f; // for NPC behavior
  private const float _maxFireDistanceFar = 18.0f; // for NPC behavior
  private const float _minFireDistanceFar = 8.0f; // for NPC behavior
  private const float _maxFireDistanceClose = 3.0f; // for NPC behavior
  private const float _minRetreatDistance = 6.0f; // for NPC behavior
  private const int _playerTankNum = 1;

  public GameObject ExplosionPrefab;
  public Rigidbody Shell;
  public Transform FireTransform;
  public Slider AimSlider;
  public Slider HealthSlider;
  public Image HealthFillImage;
  public AudioSource MovementAudio;
  public AudioSource ShootingAudio;
  public AudioClip EngineIdling;
  public AudioClip EngineDriving;
  public AudioClip ChargingClip;
  public AudioClip FireClip;
  [HideInInspector] public Color TankColor {
    get { return _color; }
    set {
      _color = value;
      foreach (MeshRenderer r in GetComponentsInChildren<MeshRenderer>()) {
        r.material.color = _color;
      }
    }
  }
  [HideInInspector] public int TankNum;
  [HideInInspector] public int TeamNum;

  private Rigidbody _rb;
  private ParticleSystem _explosion;
  private GameObject _canvas;
  private AudioSource _explosionAudio;
  private Color _color;
  private Vector3 _startingPosition;
  private Quaternion _startingRotation;
  private float _engineIdlingPitch;
  private float _engineDrivingPitch;
  private float _movementInput;
  private float _turnInput;
  private float _currentLaunchForce;
  private float _timeAtLastFire;
  private float _health;
  private bool _controlEnabled;
  private bool _fired;
  private bool _dead;
  private Tank _target; // for NPC behavior

  private void Awake() {
    _rb = GetComponent<Rigidbody>();
    _canvas = GetComponentInChildren<Canvas>().gameObject;
    _explosion = Instantiate(ExplosionPrefab).GetComponent<ParticleSystem>();
    _explosionAudio = _explosion.GetComponent<AudioSource>();
    _explosion.gameObject.SetActive(false);
    _startingPosition = transform.position;
    _startingRotation = transform.rotation;
    _engineIdlingPitch = Random.Range(MovementAudio.pitch - _pitchRange,
                                      MovementAudio.pitch);
    _engineDrivingPitch = Random.Range(MovementAudio.pitch,
                                       MovementAudio.pitch + _pitchRange);
  }

  private void OnEnable() {
    _timeAtLastFire = Time.time;
    _rb.isKinematic = _controlEnabled = _dead = false;
    _movementInput = _turnInput = 0;
    _target = null;
    _currentLaunchForce = AimSlider.value = _minLaunchForce;
    _health = _startingHealth;
  }

  private void FixedUpdate() {
    if (_controlEnabled) {
      Move();
      Turn();
    }
  }

  private void Update() {
    if (_controlEnabled) {
      if (TankNum == _playerTankNum) {
        HandlePlayerBehavior();
      } else {
        HandleNpcBehavior();
      }
      UpdateEngineAudio();
    }
  }

  private void OnDisable() {
    _rb.isKinematic = true;
  }

  public void TakeDamage(float amount) {
    _health -= amount;
    if (_health <= 0 && !_dead) {
      Explode();
    } else if (TankNum == _playerTankNum) {
      SetHealthUI();
    }
  }

  public void DisableControl() {
    _controlEnabled = false;
    MovementAudio.Stop();
    if (TankNum == _playerTankNum) {
      SetHealthUI();
    } else {
      _canvas.SetActive(false);
    }
  }

  public void EnableControl() {
    _controlEnabled = true;
    PlayIdlingAudio();
  }

  public void Reset() {
    transform.SetPositionAndRotation(_startingPosition, _startingRotation);
    gameObject.SetActive(false);
    gameObject.SetActive(true);
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

  private void HandlePlayerBehavior() {
    _movementInput = Input.GetAxis("Vertical1");
    _turnInput = Input.GetAxis("Horizontal1");
    AimSlider.value = _minLaunchForce;
    if (Input.GetKeyDown(KeyCode.Space)) {
      _fired = false;
      _currentLaunchForce = _minLaunchForce;
      ShootingAudio.clip = ChargingClip;
      ShootingAudio.Play();
    } else if (Input.GetKey(KeyCode.Space) && !_fired) {
      _currentLaunchForce += _chargeSpeed * Time.deltaTime;
      AimSlider.value = _currentLaunchForce;
    } else if (Input.GetKeyUp(KeyCode.Space) && !_fired) {
      Fire();
    } else if (_currentLaunchForce >= _maxLaunchForce && !_fired) {
      _currentLaunchForce = _maxLaunchForce;
      Fire();
    }
  }

  private void HandleNpcBehavior() {
    if (!_target || _target._dead || Random.Range(0, 500) == 0) {
      _target = GameManager.FindTargetForTank(TankNum);
    } else {
      float distance = Vector3.Distance(transform.position,
                                        _target.transform.position);

      _turnInput = transform.InverseTransformPoint(
          _target.transform.position).normalized.x;
      if (distance > _maxFireDistanceFar || (distance > _maxFireDistanceClose &&
          distance < _minRetreatDistance)) {
        _movementInput = 1.0f;
      } else if (distance < _minFireDistanceFar &&
                 distance >= _minRetreatDistance) {
        _movementInput = -1.0f;
      } else {
        _movementInput = 0;
      }
      if (_movementInput <= 0 && Mathf.Abs(_turnInput) < _maxFireAngle &&
          InFireRange(distance)) {
        Fire();
      }
    }
  }

  private void Fire() {
    if (Time.time - _timeAtLastFire < _fireDelay) {
      return;
    }
    Rigidbody shell = Instantiate(Shell, FireTransform.position,
                                  FireTransform.rotation) as Rigidbody;

    _fired = true;
    shell.velocity = _currentLaunchForce * FireTransform.forward;
    ShootingAudio.clip = FireClip;
    ShootingAudio.Play();
    _currentLaunchForce = _minLaunchForce;
    _timeAtLastFire = Time.time;
  }

  private bool InFireRange(float distance) {
    return distance <= _maxFireDistanceClose ||
        (distance <= _maxFireDistanceFar && distance >= _minFireDistanceFar);
  }

  private void UpdateEngineAudio() {
    if (Moving()) {
      if (MovementAudio.clip == EngineIdling) {
        PlayDrivingAudio();
      }
    } else if (MovementAudio.clip == EngineDriving) {
      PlayIdlingAudio();
    }
  }

  private bool Moving() {
    return Mathf.Abs(_movementInput) > 0.1f || Mathf.Abs(_turnInput) == 1.0f;
  }

  private void PlayIdlingAudio() {
    MovementAudio.clip = EngineIdling;
    MovementAudio.pitch = _engineIdlingPitch;
    MovementAudio.Play();
  }

  private void PlayDrivingAudio() {
    MovementAudio.clip = EngineDriving;
    MovementAudio.pitch = _engineDrivingPitch;
    MovementAudio.Play();
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
}
