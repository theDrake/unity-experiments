using UnityEngine;

public class RunPlayer : MonoBehaviour {
  private const float _gravityModifier = 1.5f;
  private const float _defaultTargetSpeed = 12.0f;
  private const float _speedIncrement = 0.1f;
  private const float _speedBoost = 1.5f;
  private const float _jumpForce = 700.0f;
  private const int _maxJumps = 2;

  public ParticleSystem explosion;
  public ParticleSystem dirt;
  public AudioClip jumpSound;
  public AudioClip crashSound;
  [HideInInspector] public float speed;
  [HideInInspector] public bool gameOver;

  private Rigidbody _rb;
  private Animator _animator;
  private AudioSource _audioSource;
  private float _score;
  private float _targetSpeed;
  private int _jumpState;

  private void Start() {
    Physics.gravity *= _gravityModifier;
    _targetSpeed = _defaultTargetSpeed;
    _rb = GetComponent<Rigidbody>();
    _animator = GetComponent<Animator>();
    _audioSource = GetComponent<AudioSource>();
  }

  private void Update() {
    if (!gameOver) {
      if (Input.GetKeyDown(KeyCode.LeftShift)) {
        _targetSpeed *= _speedBoost;
        _animator.SetFloat("Speed_f", 2.0f);
      } else if (Input.GetKeyUp(KeyCode.LeftShift)) {
        _targetSpeed /= _speedBoost;
        _animator.SetFloat("Speed_f", 1.5f);
      }
      if (speed < _targetSpeed) {
        speed += _speedIncrement;
      } else if (speed > _targetSpeed) {
        speed -= _speedIncrement;
      }
      if (_jumpState < _maxJumps && Input.GetKeyDown(KeyCode.Space)) {
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        _animator.SetTrigger("Jump_trig");
        dirt.Stop();
        _audioSource.PlayOneShot(jumpSound, 0.6f);
        ++_jumpState;
      }
      _score += speed * Time.deltaTime;
      Debug.Log("Score: " + (int) _score);
    }
  }

  private void OnCollisionEnter(Collision c) {
    if (!gameOver) {
      if (c.gameObject.CompareTag("Ground")) {
        _jumpState = 0;
        dirt.Play();
      } else if (c.gameObject.CompareTag("Obstacle")) {
        gameOver = true;
        dirt.Stop();
        _audioSource.PlayOneShot(crashSound, 2.0f);
        explosion.Play();
        _animator.SetBool("Death_b", true);
        _animator.SetInteger("DeathType_int", Random.Range(1, 2));
        Debug.Log("Game over! Score: " + (int) _score);
      }
    }
  }
}
