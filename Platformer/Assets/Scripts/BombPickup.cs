using UnityEngine;
using System.Collections;

public class BombPickup : MonoBehaviour {
  public AudioClip pickupClip;  // Sound for when the bomb crate is picked up.

  private Animator anim;  // Reference to the animator component.
  private bool landed = false;  // Whether the crate has landed yet.

  void Awake() {
    anim = transform.root.GetComponent<Animator>();
  }

  void OnTriggerEnter2D(Collider2D other) {
    if (other.tag == "Player") {
      AudioSource.PlayClipAtPoint(pickupClip, transform.position);
      other.GetComponent<LayBombs>().bombCount++;
      Destroy(transform.root.gameObject);
    } else if (other.tag == "ground" && !landed) {
      anim.SetTrigger("Land");
      transform.parent = null;
      gameObject.AddComponent<Rigidbody2D>();
      landed = true;
    }
  }
}
