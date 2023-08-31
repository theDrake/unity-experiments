using System.Collections;
using UnityEngine;

public class BattlePlayer : MonoBehaviour {
  public float speed, powerupDuration, blastForce;
  public GameObject chargeIndicator, shieldIndicator, starBullet;

  private Rigidbody _rb;
  private GameObject _focalPoint;
  private Vector3 _normalScale;
  private Vector3 __enlargedScale;
  private float _normalMass;
  private float __enlargedMass;
  private float _normalSpeed;
  private float __enlargedSpeed;
  private float _sizeChangeElapsedTime;
  private float _lerpStep;
  private float _starElapsedTime;
  private const float _enlargementDuration = 1.0f; // seconds
  private const float _starBulletDelay = 0.25f; // seconds
  private const float _boundary = 30.0f;
  private bool _charged = false;
  private bool _shielded = false;
  private bool _starred = false;
  private bool _enlarged = false;
  private bool _shrinking = false;

  private void Start() {
    _rb = GetComponent<Rigidbody>();
    _focalPoint = GameObject.Find("Focal Point");
    _normalScale = transform.localScale;
    __enlargedScale = _normalScale * 2;
    _normalMass = _rb.mass;
    __enlargedMass = _normalMass * 3;
    _normalSpeed = speed;
    __enlargedSpeed = _normalSpeed * 2;
  }

  private void FixedUpdate() {
    if (transform.position.magnitude > _boundary) {
      transform.position = new(0, 1, 0);
      _rb.Sleep();
    } else {
      _rb.AddForce(_focalPoint.transform.forward * Input.GetAxis("Vertical") *
                   speed);
      _rb.AddForce(_focalPoint.transform.right * Input.GetAxis("Horizontal") *
                   speed);
      if (Input.GetKeyDown(KeyCode.Space) && Grounded()) {
        _rb.AddForce(Vector3.up * speed / 2, ForceMode.Impulse);
      }
      if (_starred) {
        _starElapsedTime += Time.deltaTime;
        if (_starElapsedTime > _starBulletDelay) {
          Instantiate(starBullet, transform.position,
                      starBullet.transform.rotation);
          _starElapsedTime = 0;
        }
      }
    }
    if (_enlarged && _sizeChangeElapsedTime < _enlargementDuration) {
      _sizeChangeElapsedTime += Time.deltaTime;
      _lerpStep = Mathf.SmoothStep(0, 1, _sizeChangeElapsedTime /
                                   _enlargementDuration);
      transform.localScale = Vector3.Lerp(_normalScale, __enlargedScale,
                                          _lerpStep);
      _rb.mass = Mathf.Lerp(_normalMass, __enlargedMass, _lerpStep);
      speed = Mathf.Lerp(_normalSpeed, __enlargedSpeed, _lerpStep);
    } else if (_shrinking && _sizeChangeElapsedTime < _enlargementDuration) {
      _sizeChangeElapsedTime += Time.deltaTime;
      _lerpStep = Mathf.SmoothStep(0, 1, _sizeChangeElapsedTime /
                                   _enlargementDuration);
      transform.localScale = Vector3.Lerp(__enlargedScale, _normalScale,
                                          _lerpStep);
      _rb.mass = Mathf.Lerp(__enlargedMass, _normalMass, _lerpStep);
      speed = Mathf.Lerp(__enlargedSpeed, _normalSpeed, _lerpStep);
      if (_sizeChangeElapsedTime >= _enlargementDuration) {
        _shrinking = false;
      }
    }
  }

  // private void OnCollisionEnter(Collision c) {
  //   if (c.gameObject.CompareTag("Enemy")) {
  //     Rigidbody enemyRb = c.gameObject.GetComponent<Rigidbody>();
  //     Vector3 away = (enemyRb.position - transform.position).normalized;
  //     if (_charged) {
  //       enemyRb.AddForce(Vector3.up * chargeForce, ForceMode.Impulse);
  //       // enemyRb.AddForce(away * chargeForce, ForceMode.Impulse);
  //     }
  //     if (_shielded) {
  //       // enemyRb.AddForce(Vector3.up * shieldForce, ForceMode.Impulse);
  //       enemyRb.AddForce(away * shieldForce, ForceMode.Impulse);
  //     }
  //   }
  // }

  private void OnTriggerEnter(Collider other) {
    if (other.CompareTag("Lightning") && !_charged) {
      Destroy(other.gameObject);
      _charged = true;
      chargeIndicator.SetActive(true);
      StartCoroutine(ChargeCountdown());
    } else if (other.CompareTag("Gem") && !_shielded) {
      Destroy(other.gameObject);
      _shielded = true;
      shieldIndicator.SetActive(true);
      StartCoroutine(ShieldCountdown());
    } else if (other.CompareTag("Star") && !_starred) {
      Destroy(other.gameObject);
      _starred = true;
      _starElapsedTime = _starBulletDelay;
      StartCoroutine(StarCountdown());
    } else if (other.CompareTag("Multiplier") && !_enlarged && !_shrinking) {
      Destroy(other.gameObject);
      _enlarged = true;
      _sizeChangeElapsedTime = 0;
      StartCoroutine(EnlargementCountdown());
    } else if (other.CompareTag("Blast")) {
      Destroy(other.gameObject);
      BattleEnemy[] enemies = FindObjectsByType<BattleEnemy>(
          FindObjectsSortMode.None);
      // Vector3 away;
      foreach (BattleEnemy e in enemies) {
        // away = (e.transform.position - transform.position).normalized;
        // e.GetComponent<Rigidbody>().AddForce(away * blastForce,
        //                                      ForceMode.Impulse);
        e.GetComponent<Rigidbody>().AddForce(Vector3.up * blastForce,
                                             ForceMode.Impulse);
      }
    }
  }

  private IEnumerator ChargeCountdown() {
    yield return new WaitForSeconds(powerupDuration);
    chargeIndicator.SetActive(false);
    _charged = false;
  }

  private IEnumerator ShieldCountdown() {
    yield return new WaitForSeconds(powerupDuration);
    shieldIndicator.SetActive(false);
    _shielded = false;
  }

  private IEnumerator StarCountdown() {
    yield return new WaitForSeconds(powerupDuration);
    _starred = false;
    // BattleStarBullet[] bullets = FindObjectsByType<BattleStarBullet>(
    //     FindObjectsSortMode.None);
    // foreach (BattleStarBullet b in bullets) {
    //   Destroy(b.gameObject);
    // }
  }

  private IEnumerator EnlargementCountdown() {
    yield return new WaitForSeconds(powerupDuration);
    _enlarged = false;
    _shrinking = true;
    _sizeChangeElapsedTime = 0;
  }

  private bool Grounded() {
    return Physics.Raycast(transform.position, Vector3.down,
                           GetComponent<Collider>().bounds.extents.y + 0.1f);
  }
}
