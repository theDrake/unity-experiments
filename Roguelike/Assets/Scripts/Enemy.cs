using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {
  public int attackPower;
  public AudioClip attackSound1, attackSound2;

  private Animator animator;
  private Transform target;
  private bool skipMove;

  protected override void Start() {
    GameManager.instance.AddEnemy(this);
    animator = GetComponent<Animator>();
    target = GameObject.FindGameObjectWithTag("Player").transform;
    base.Start();
  }

  protected override void AttemptMove<T>(int xDir, int yDir) {
    if (skipMove) {
      skipMove = false;
    } else {
      base.AttemptMove<T>(xDir, yDir);
      skipMove = true;
    }
  }

  public void Move() {
    int xDir = 0;
    int yDir = 0;

    if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon) {
      yDir = (target.position.y > transform.position.y) ? 1 : -1;
    } else {
      xDir = (target.position.x > transform.position.x) ? 1 : -1;
    }
    AttemptMove<Player>(xDir, yDir);
  }

  protected override void OnCantMove<T>(T component) {
    Player player = component as Player;
    animator.SetTrigger("enemyAttack");
    SoundManager.instance.RandomizeSfx(attackSound1, attackSound2);
    player.TakeDamage(attackPower);
  }
}
