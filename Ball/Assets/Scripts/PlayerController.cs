using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
  public float speed;

  private Rigidbody rb;

  void Start() {
    rb = GetComponent<Rigidbody>();
  }

  void FixedUpdate() {
    float moveHorizontal = Input.GetAxis("Horizontal"),
          moveVertical = Input.GetAxis("Vertical");
    Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

    rb.AddForce(movement * speed);
  }

  void OnTriggerEnter(Collider other) {
    //Destroy(other.gameObject);
    if (other.gameObject.CompareTag("Powerup")) {
      other.gameObject.SetActive(false);
    }
  }
}
