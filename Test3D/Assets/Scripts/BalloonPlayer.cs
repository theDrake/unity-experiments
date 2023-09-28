using UnityEngine;

public class BalloonPlayer : MonoBehaviour {
  private const float _upwardForce = 100.0f;
  private const float _initialUpwardForce = 5.0f;
  private const float _gravityModifier = 1.5f;
  private const float _minY = 1.0f;
  private const float _maxY = 14.5f;

  public ParticleSystem explosion;
  public ParticleSystem fireworks;
  public AudioClip moneySound;
  public AudioClip explodeSound;
  public bool gameOver;

  private Rigidbody _rb;
  private AudioSource _playerAudio;

  private void Start() {
    gameOver = false;
    Physics.gravity *= _gravityModifier;
    _rb = GetComponent<Rigidbody>();
    _playerAudio = GetComponent<AudioSource>();
    _rb.AddForce(Vector3.up * _initialUpwardForce, ForceMode.Impulse);
  }

  private void Update() {
    if (!gameOver && Input.GetKey(KeyCode.Space)) {
      _rb.AddForce(Vector3.up * _upwardForce);
    }
    if (transform.position.y > _maxY) {
      transform.SetPositionAndRotation(new(transform.position.x, _maxY,
                                           transform.position.z),
                                       transform.rotation);
    } else if (transform.position.y < _minY) {
      transform.SetPositionAndRotation(new(transform.position.x, _minY,
                                           transform.position.z),
                                       transform.rotation);
    }
  }

  private void OnCollisionEnter(Collision other) {
    if (other.gameObject.CompareTag("Bomb")) {
      explosion.Play();
      _playerAudio.PlayOneShot(explodeSound, 1.0f);
      gameOver = true;
      Debug.Log("Game Over");
      Destroy(other.gameObject);
    } else if (other.gameObject.CompareTag("Money")) {
      fireworks.Play();
      _playerAudio.PlayOneShot(moneySound, 1.0f);
      Destroy(other.gameObject);
    }
  }
}
