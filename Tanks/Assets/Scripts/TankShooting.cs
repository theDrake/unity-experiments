using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour {
  public int PlayerNumber = 1;
  public Rigidbody Shell;
  public Transform FireTransform;
  public Slider AimSlider;
  public AudioSource ShootingAudio;
  public AudioClip ChargingClip;
  public AudioClip FireClip;

  private const float _minLaunchForce = 15.0f;
  private const float _maxLaunchForce = 30.0f;
  private const float _maxChargeTime = 0.75f;
  private float _currentLaunchForce;
  private float _chargeSpeed;
  private string _fireButton;
  private bool _fired;

  private void OnEnable() {
    _currentLaunchForce = _minLaunchForce;
    AimSlider.value = _minLaunchForce;
  }

  private void Start() {
    _fireButton = "Fire" + PlayerNumber;
    _chargeSpeed = (_maxLaunchForce - _minLaunchForce) / _maxChargeTime;
  }

  private void Update() {
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

  private void Fire() {
    _fired = true;
    Rigidbody shell = Instantiate(Shell, FireTransform.position,
                                  FireTransform.rotation) as Rigidbody;
    shell.velocity = _currentLaunchForce * FireTransform.forward;
    ShootingAudio.clip = FireClip;
    ShootingAudio.Play();
    _currentLaunchForce = _minLaunchForce;
  }
}
