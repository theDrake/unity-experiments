using UnityEngine;

public class Enemy : MonoBehaviour {
  private const float _attackRate = 0.8f;
  private const float _initialAttackDelay = 0.5f;
  private const float _speed = 4.0f;
  private const float _tilt = -4.0f;

  public GameObject shot1, shot2;
  public Transform shotSpawn1, shotSpawn2;
  public Boundary boundary;

  private Rigidbody _rb;
  private AudioSource _audio;
  private Transform _player;

  private void Start() {
    _player = GameObject.FindWithTag("Player").transform;
    _rb = GetComponent<Rigidbody>();
    _audio = GetComponent<AudioSource>();
    InvokeRepeating(nameof(Attack), _initialAttackDelay, _attackRate);
  }

  private void FixedUpdate() {
    _rb.velocity = new Vector3(_player.position.x - transform.position.x, 0,
                               -1.0f) * _speed;
    transform.SetPositionAndRotation(new(
        Mathf.Clamp(transform.position.x, boundary.xMin, boundary.xMax), 0,
        Mathf.Clamp(transform.position.z, boundary.zMin, boundary.zMax)),
        Quaternion.Euler(0, 0, _rb.velocity.x * _tilt));
  }

  private void Attack() {
    if (GameManager.Playing) {
      Instantiate(shot1, shotSpawn1.position, shotSpawn1.rotation);
      Instantiate(shot2, shotSpawn2.position, shotSpawn2.rotation);
      _audio.Play();
    } else {
      CancelInvoke();
    }
  }
}
