using UnityEngine;
using System.Collections;

public class HealthPickup : MonoBehaviour {
  public float healthBonus;  // How much health the crate gives the player.
  public AudioClip collect;  // Sound of the crate being collected.

  private PickupSpawner pickupSpawner;  // Reference to the pickup spawner.
  private Animator anim;  // Reference to the animator component.
  private bool landed;  // Whether or not the crate has landed.

  void Awake() {
    pickupSpawner =
      GameObject.Find("pickupManager").GetComponent<PickupSpawner>();
    anim = transform.root.GetComponent<Animator>();
  }

  void OnTriggerEnter2D(Collider2D other) {
    if (other.tag == "Player") {
      PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
      playerHealth.health += healthBonus;
      playerHealth.health = Mathf.Clamp(playerHealth.health, 0f, 100f);
      playerHealth.UpdateHealthBar();
      pickupSpawner.StartCoroutine(pickupSpawner.DeliverPickup());
      AudioSource.PlayClipAtPoint(collect, transform.position);
      Destroy(transform.root.gameObject);
    } else if (other.tag == "ground" && !landed) {
      anim.SetTrigger("Land");
      transform.parent = null;
      gameObject.AddComponent<Rigidbody2D>();
      landed = true;
    }
  }
}
