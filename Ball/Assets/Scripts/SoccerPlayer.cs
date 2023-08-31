using System.Collections;
using UnityEngine;

public class SoccerPlayer : MonoBehaviour {
  public int powerupDuration;
  public float speed, strength;
  public GameObject chargeIndicator;
  public ParticleSystem smoke;

  private Rigidbody _rb;
  private GameObject _focalPoint;
  private bool _boosted = false;

  private void Start() {
    _rb = GetComponent<Rigidbody>();
    _focalPoint = GameObject.Find("Focal Point");
  }

  private void FixedUpdate() {
    if (_boosted) {
      if (Input.GetKeyUp(KeyCode.Space)) {
        _boosted = false;
        speed /= 2;
      } else {
        smoke.Play();
      }
    } else if (Input.GetKeyDown(KeyCode.Space)) {
      _boosted = true;
      speed *= 2;
    }
    _rb.AddForce(Input.GetAxis("Vertical") * speed * Time.deltaTime *
                 _focalPoint.transform.forward);
    _rb.AddForce(Input.GetAxis("Horizontal") * speed * Time.deltaTime *
                 _focalPoint.transform.right);
    chargeIndicator.transform.position = transform.position;
  }

  private void OnTriggerEnter(Collider other) {
    if (other.gameObject.CompareTag("Powerup")) {
      Destroy(other.gameObject);
      strength *= 2;
      chargeIndicator.SetActive(true);
      StartCoroutine(ChargeCountdown());
    }
  }

  private void OnCollisionEnter(Collision other) {
    if (other.gameObject.CompareTag("Enemy")) {
      Rigidbody enemyRb = other.gameObject.GetComponent<Rigidbody>();
      Vector3 awayFromPlayer = (other.gameObject.transform.position -
                                transform.position).normalized;
      enemyRb.AddForce(strength * awayFromPlayer, ForceMode.Impulse);
    }
  }

  private IEnumerator ChargeCountdown() {
    yield return new WaitForSeconds(powerupDuration);
    strength /= 2;
    chargeIndicator.SetActive(false);
  }
}
