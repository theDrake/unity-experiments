using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {
  public Sprite damageSprite;
  public AudioClip damageSound1, damageSound2;
  public int hp = 4;

  private SpriteRenderer spriteRenderer;

  void Awake() {
    spriteRenderer = GetComponent<SpriteRenderer>();
  }

  public void DamageWall(int damage) {
    spriteRenderer.sprite = damageSprite;
    SoundManager.instance.RandomizeSfx(damageSound1, damageSound2);
    hp -= damage;
    if (hp <= 0) {
      gameObject.SetActive(false);
    }
  }
}
