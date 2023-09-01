using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour {
  public Slider Slider;
  public Image FillImage;
  public GameObject ExplosionPrefab;

  private AudioSource _explosionAudio;
  private ParticleSystem _explosion;
  private const float _startingHealth = 100.0f;
  private readonly Color _fullHealthColor = Color.green;
  private readonly Color _zeroHealthColor = Color.red;
  private float _health;
  private bool _dead;

  private void Awake() {
    _explosion = Instantiate(ExplosionPrefab).GetComponent<ParticleSystem>();
    _explosionAudio = _explosion.GetComponent<AudioSource>();
    _explosion.gameObject.SetActive(false);
  }

  private void OnEnable() {
    _health = _startingHealth;
    _dead = false;
    SetHealthUI();
  }

  public void TakeDamage(float amount) {
    _health -= amount;
    SetHealthUI();
    if (_health <= 0 && !_dead) {
      Explode();
    }
  }

  private void SetHealthUI() {
    Slider.value = _health;
    FillImage.color = Color.Lerp(_zeroHealthColor, _fullHealthColor,
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
