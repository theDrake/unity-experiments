using UnityEngine;

public class Destructible : MonoBehaviour {
  private GameObject _explosionPrefab;
  private ParticleSystem _explosion;
  private AudioSource _explosionAudio;
  [SerializeField] private float _health = 500.0f;
  private float _startingHealth;
  private bool _destroyed;

  private void Awake() {
    _explosionPrefab = Resources.Load<GameObject>("Prefabs/Explosion");
    _explosion = Instantiate(_explosionPrefab).GetComponent<ParticleSystem>();
    _explosionAudio = _explosion.GetComponent<AudioSource>();
    _explosion.gameObject.SetActive(false);
    _startingHealth = _health;
  }

  private void OnEnable() {
    _health = _startingHealth;
    _destroyed = false;
  }

  private void OnCollisionStay(Collision c) {
    TakeDamage(c.impulse.magnitude);
  }

  public void TakeDamage(float amount) {
    _health -= amount;
    if (_health <= 0 && !_destroyed) {
      Explode();
    }
  }

  public void Reset() {
    gameObject.SetActive(false);
    gameObject.SetActive(true);
  }

  private void Explode() {
    _destroyed = true;
    _explosion.transform.position = transform.position;
    _explosion.gameObject.SetActive(true);
    _explosion.Play();
    _explosionAudio.Play();
    gameObject.SetActive(false);
  }
}
