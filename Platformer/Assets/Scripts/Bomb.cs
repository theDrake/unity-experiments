using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {
  public float bombRadius = 10f;  // Radius within which enemies are killed.
  public float bombForce = 100f;  // Force against enemies hit by blast.
  public AudioClip boom;  // Audioclip of explosion.
  public AudioClip fuse;  // Audioclip of fuse.
  public float fuseTime = 1.5f;
  public GameObject explosion;  // Prefab of explosion effect.

  private LayBombs layBombs;
  private PickupSpawner pickupSpawner;
  private ParticleSystem explosionFX;

  void Awake() {
    explosionFX =
      GameObject.FindGameObjectWithTag(
        "ExplosionFX"
      ).GetComponent<ParticleSystem>();
    pickupSpawner =
      GameObject.Find("pickupManager").GetComponent<PickupSpawner>();
    if (GameObject.FindGameObjectWithTag("Player"))
      layBombs =
        GameObject.FindGameObjectWithTag("Player").GetComponent<LayBombs>();
  }

  void Start() {
    if (transform.root == transform)
      StartCoroutine(BombDetonation());
  }

  IEnumerator BombDetonation() {
    AudioSource.PlayClipAtPoint(fuse, transform.position);
    yield return new WaitForSeconds(fuseTime);
    Explode();
  }

  public void Explode() {
    layBombs.bombLaid = false;
    pickupSpawner.StartCoroutine(pickupSpawner.DeliverPickup());
    Collider2D[] enemies =
      Physics2D.OverlapCircleAll(transform.position, bombRadius,
                                 1 << LayerMask.NameToLayer("Enemies"));
    foreach (Collider2D en in enemies) {
      Rigidbody2D rb = en.GetComponent<Rigidbody2D>();
      if (rb != null && rb.tag == "Enemy") {
        rb.gameObject.GetComponent<Enemy>().HP = 0;
        Vector3 deltaPos = rb.transform.position - transform.position;
        Vector3 force = deltaPos.normalized * bombForce;
        rb.AddForce(force);
      }
    }
    explosionFX.transform.position = transform.position;
    explosionFX.Play();
    Instantiate(explosion, transform.position, Quaternion.identity);
    AudioSource.PlayClipAtPoint(boom, transform.position);
    Destroy(gameObject);
  }
}
