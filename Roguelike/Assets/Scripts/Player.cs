using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MovingObject {
  public int attackPower = 1;
  public int hpPerFood = 10;
  public int hpPerSoda = 20;
  public float restartLevelDelay = 1.0f;
  public Text hpText;
  public AudioClip moveSound1, moveSound2, eatSound1, eatSound2, drinkSound1,
    drinkSound2, gameOverSound;

  private Animator animator;
  private int hp;

#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_PHONE
  private Vector2 touchOrigin = -Vector2.one;
#endif

  protected override void Start() {
    animator = GetComponent<Animator>();
    hp = GameManager.instance.playerHp;
    hpText.text = "HP: " + hp;
    base.Start();
  }

  private void OnDisable() {
    GameManager.instance.playerHp = hp;
  }

  private void Update() {
    if (GameManager.instance.playersTurn) {
      int horizontal = 0;
      int vertical = 0;
#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_PHONE
      if (Input.touchCount > 0) {
        Touch myTouch = Input.touches[0];
        if (myTouch.phase == TouchPhase.Began) {
          touchOrigin = myTouch.position;
        } else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0) {
          Vector2 touchEnd = myTouch.position;
          float x = touchEnd.x - touchOrigin.x;
          float y = touchEnd.y - touchOrigin.y;
          touchOrigin.x = -1; // ensure "else if" above is false next time
          if (Mathf.Abs(x) > Mathf.Abs(y)) {
            horizontal = x > 0 ? 1 : -1;
          } else {
            vertical = y > 0 ? 1 : -1;
          }
        }
      }
#else // UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
      horizontal = (int) Input.GetAxisRaw("Horizontal");
      vertical = (int) Input.GetAxisRaw("Vertical");
      if (horizontal != 0) {
        vertical = 0;
      }
#endif
      if (horizontal != 0 || vertical != 0) {
        AttemptMove<Wall>(horizontal, vertical);
      }
    }
  }

  protected override void AttemptMove<T>(int xDir, int yDir) {
    hp--;
    hpText.text = "HP: " + hp;
    base.AttemptMove<T>(xDir, yDir);
    RaycastHit2D hit;
    if (Move(xDir, yDir, out hit)) {
      SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
    }
    CheckForGameOver();
    GameManager.instance.playersTurn = false;
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.tag == "Exit") {
      Invoke("Restart", restartLevelDelay);
      enabled = false;
    } else if (other.tag == "Food") {
      hp += hpPerFood;
      hpText.text = "+" + hpPerFood + " HP: " + hp;
      SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);
      other.gameObject.SetActive(false);
    } else if (other.tag == "Soda") {
      hp += hpPerSoda;
      hpText.text = "+" + hpPerSoda + " HP: " + hp;
      SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);
      other.gameObject.SetActive(false);
    }
  }

  protected override void OnCantMove<T>(T component) {
    Wall wall = component as Wall;
    wall.DamageWall(attackPower);
    animator.SetTrigger("playerAttack");
  }

  private void Restart() {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex,
                           LoadSceneMode.Single);
  }

  public void TakeDamage(int damage) {
    animator.SetTrigger("playerDamaged");
    hp -= damage;
    hpText.text = "-" + damage + " HP: " + hp;
    CheckForGameOver();
  }

  private void CheckForGameOver() {
    if (hp <= 0) {
      gameObject.SetActive(false);
      SoundManager.instance.musicSource.Stop();
      SoundManager.instance.PlaySingle(gameOverSound);
      GameManager.instance.GameOver();
    }
  }
}
