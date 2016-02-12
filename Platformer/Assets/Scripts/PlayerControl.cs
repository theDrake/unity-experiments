using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
  [HideInInspector]
  public bool facingRight = true;
  [HideInInspector]
  public bool jump = false;

  public float moveForce = 365f;  // Force added to move player left/right.
  public float maxSpeed = 5f;  // Along x-axis.
  public AudioClip[] jumpClips;  // Array of clips for when the player jumps.
  public float jumpForce = 1000f;  // Force added when the player jumps.
  public AudioClip[] taunts;  // Array of clips for when the player taunts.
  public float tauntProbability = 50f;  // Chance of a taunt happening.
  public float tauntDelay = 1f;  // Delay for when the taunt should happen.

  private int tauntIndex;  // Index of most recent taunt.
  private Transform groundCheck;  // Where to check if the player is grounded.
  private bool grounded = false;  // Whether or not the player's on the ground.
  private Animator anim;  // Reference to the player's animator component.

  void Awake() {
    groundCheck = transform.Find("groundCheck");
    anim = GetComponent<Animator>();
  }

  void Update() {
    grounded = Physics2D.Linecast(transform.position, groundCheck.position,
                                  1 << LayerMask.NameToLayer("Ground"));
    if (Input.GetButtonDown("Jump") && grounded) {
      jump = true;
    }
  }

  void FixedUpdate() {
    float h = Input.GetAxis("Horizontal");
    anim.SetFloat("Speed", Mathf.Abs(h));
    if (h * GetComponent<Rigidbody2D>().velocity.x < maxSpeed) {
      GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);
    }
    if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed) {
      GetComponent<Rigidbody2D>().velocity =
        new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) *
                      maxSpeed,
                    GetComponent<Rigidbody2D>().velocity.y);
    }
    if (h > 0 && !facingRight) {
      Flip();
    } else if (h < 0 && facingRight) {
      Flip();
    }
    if (jump) {
      anim.SetTrigger("Jump");
      int i = Random.Range(0, jumpClips.Length);
      AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);
      GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
      jump = false;
    }
  }

  void Flip() {
    facingRight = !facingRight;
    Vector3 theScale = transform.localScale;
    theScale.x *= -1;
    transform.localScale = theScale;
  }

  public IEnumerator Taunt() {
    float tauntChance = Random.Range(0f, 100f);
    if (tauntChance > tauntProbability) {
      yield return new WaitForSeconds(tauntDelay);
      if (!GetComponent<AudioSource>().isPlaying) {
        tauntIndex = TauntRandom();
        GetComponent<AudioSource>().clip = taunts[tauntIndex];
        GetComponent<AudioSource>().Play();
      }
    }
  }

  int TauntRandom() {
    int i = Random.Range(0, taunts.Length);
    if (i == tauntIndex) {
      return TauntRandom();
    } else {
      return i;
    }
  }
}
