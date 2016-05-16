using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
  public GameObject shot1, shot2;
  public Transform shotSpawn1, shotSpawn2;
  public Boundary boundary;
  public float attackRate, initialAttackDelay, speed, tilt;

  private Rigidbody rb;
  private AudioSource audioSource;
  private Transform playerTransform;

  void Start() {
    playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    rb = GetComponent<Rigidbody>();
    audioSource = GetComponent<AudioSource>();
    InvokeRepeating("Attack", initialAttackDelay, attackRate);
  }

  void FixedUpdate() {
    Vector3 movement;

    if (playerTransform != null) {
      movement = new Vector3(playerTransform.position.x - transform.position.x,
                             0.0f,
                             -1.0f);
      rb.velocity = movement * speed;
      rb.position = new Vector3(
          Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
          0.0f,
          Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax));
      rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * tilt);
    }
  }

  void Attack() {
    Instantiate(shot1, shotSpawn1.position, shotSpawn1.rotation);
    Instantiate(shot2, shotSpawn2.position, shotSpawn2.rotation);
    audioSource.Play();
  }
}
