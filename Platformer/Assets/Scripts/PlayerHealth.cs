using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
  public float health = 100f;  // The player's health.
  public float repeatDamagePeriod = 2f;  // How frequently player can be hurt.
  public AudioClip[] ouchClips;  // Clips to play when player is hurt.
  public float hurtForce = 10f;  // Force to push player when hurt.
  public float damageAmount = 10f;  // When enemies touch the player.

  private SpriteRenderer healthBar;  // Reference to health sprite renderer.
  private float lastHitTime;  // Time at which player was last hit.
  private Vector3 healthScale;  // Local scale of initial health bar.
  private PlayerControl playerControl;  // Reference to PlayerControl script.
  private Animator anim;  // Reference to the Animator on the player.

  void Awake() {
    playerControl = GetComponent<PlayerControl>();
    healthBar = GameObject.Find("HealthBar").GetComponent<SpriteRenderer>();
    anim = GetComponent<Animator>();

    // Getting intial scale of healthbar (while player has full health).
    healthScale = healthBar.transform.localScale;
  }

  void OnCollisionEnter2D(Collision2D col) {
    if (col.gameObject.tag == "Enemy") {
      if (Time.time > lastHitTime + repeatDamagePeriod) {
        if (health > 0f) {
          TakeDamage(col.transform);
          lastHitTime = Time.time;
        } else {
          Collider2D[] cols = GetComponents<Collider2D>();
          foreach (Collider2D c in cols) {
            c.isTrigger = true;
          }
          SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();
          foreach (SpriteRenderer s in spr) {
            s.sortingLayerName = "UI";
          }
          GetComponent<PlayerControl>().enabled = false;
          GetComponentInChildren<Gun>().enabled = false;
          anim.SetTrigger("Die");
        }
      }
    }
  }

  void TakeDamage(Transform enemy) {
    playerControl.jump = false;
    Vector3 hurtVector = transform.position - enemy.position + Vector3.up * 5f;
    GetComponent<Rigidbody2D>().AddForce(hurtVector * hurtForce);
    health -= damageAmount;
    UpdateHealthBar();
    int i = Random.Range(0, ouchClips.Length);
    AudioSource.PlayClipAtPoint(ouchClips[i], transform.position);
  }

  public void UpdateHealthBar() {
    healthBar.material.color = Color.Lerp(Color.green, Color.red,
                                          1 - health * 0.01f);
    healthBar.transform.localScale =
      new Vector3(healthScale.x * health * 0.01f, 1, 1);
  }
}
