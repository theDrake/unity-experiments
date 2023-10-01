using UnityEngine;

public class Wall : MonoBehaviour {
  [SerializeField] private Sprite _damageSprite;
  [SerializeField] private AudioClip _damageSound1;
  [SerializeField] private AudioClip _damageSound2;
  private SpriteRenderer _spriteRenderer;
  private int _hp = 4;

  private void Awake() {
    _spriteRenderer = GetComponent<SpriteRenderer>();
  }

  public void DamageWall(int damage) {
    _spriteRenderer.sprite = _damageSprite;
    SoundManager.Instance.PlayRandomClip(_damageSound1, _damageSound2);
    _hp -= damage;
    if (_hp <= 0) {
      Destroy(gameObject);
    }
  }
}
