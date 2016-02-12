using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {
  public float speed;
  public Text scoreText;

  private int score;
  private Rigidbody rb;

  void Start() {
    rb = GetComponent<Rigidbody>();
    score = 0;
    UpdateScoreText();
  }

  void FixedUpdate() {
    Vector3 movement = new Vector3(Input.GetAxis("Horizontal"),
                                   0.0f,
                                   Input.GetAxis("Vertical"));
    rb.AddForce(movement * speed);
  }

  void OnTriggerEnter(Collider other) {
    //Destroy(other.gameObject);
    if (other.gameObject.CompareTag("Powerup")) {
      other.gameObject.SetActive(false);
      score += 10;
      UpdateScoreText();
    }
  }

  void UpdateScoreText() {
    scoreText.text = "Score: " + score.ToString();
  }
}
