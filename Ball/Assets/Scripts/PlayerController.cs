using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
  public float speed;
  public Text scoreText;

  private int _score;
  private Rigidbody _rb;

  private void Start() {
    _rb = GetComponent<Rigidbody>();
    _score = 0;
    UpdateScoreText();
  }

  private void FixedUpdate() {
    Vector3 movement = new(Input.GetAxis("Horizontal"), 0,
                           Input.GetAxis("Vertical"));
    _rb.AddForce(movement * speed);
  }

  private void OnTriggerEnter(Collider other) {
    //Destroy(other.gameObject);
    if (other.gameObject.CompareTag("Powerup")) {
      other.gameObject.SetActive(false);
      _score += 10;
      UpdateScoreText();
    }
  }

  private void UpdateScoreText() {
    scoreText.text = "Score: " + _score.ToString();
  }
}
