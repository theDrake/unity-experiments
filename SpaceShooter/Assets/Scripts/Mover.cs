using UnityEngine;

public class Mover : MonoBehaviour {
  public float speed;

  private void Start() {
    GetComponent<Rigidbody>().linearVelocity = transform.forward * speed;
  }
}
