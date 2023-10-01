using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : GameCharacter {
  private const int _maxHp = 100;
  private const int _hpPerFood = 10;
  private const int _hpPerSoda = 20;

  [SerializeField] private Text _hpText;
  [SerializeField] private AudioClip _moveSound1;
  [SerializeField] private AudioClip _moveSound2;
  [SerializeField] private AudioClip _eatSound1;
  [SerializeField] private AudioClip _eatSound2;
  [SerializeField] private AudioClip _drinkSound1;
  [SerializeField] private AudioClip _drinkSound2;
  [SerializeField] private AudioClip _gameOverSound;
  private int _hp;

#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_PHONE
  private Vector2 _touchOrigin = -Vector2.one;
#endif

  private void Awake() {
    _animator = GetComponent<Animator>();
    _hp = _maxHp;
  }

  protected override void Start() {
    _hpText.text = "HP: " + _hp;
    base.Start();
  }

  private void Update() {
    if (GameManager.Instance.PlayersTurn) {
      int horizontal = 0, vertical = 0;

#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_PHONE
      if (Input.touchCount > 0) {
        Touch myTouch = Input.touches[0];
        if (myTouch.phase == TouchPhase.Began) {
          _touchOrigin = myTouch.position;
        } else if (myTouch.phase == TouchPhase.Ended && _touchOrigin.x >= 0) {
          Vector2 touchEnd = myTouch.position;
          float x = touchEnd.x - _touchOrigin.x;
          float y = touchEnd.y - _touchOrigin.y;

          _touchOrigin.x = -1; // ensure "else if" above is false next time
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
        MoveOrAttack<Wall>(horizontal, vertical);
      }
    }
  }

  public void TakeDamage(int damage) {
    _animator.SetTrigger("playerDamaged");
    _hp -= damage;
    _hpText.text = "-" + damage + " HP: " + _hp;
    CheckForGameOver();
  }

  protected override void MoveOrAttack<T>(int x, int y) {
    _hpText.text = "HP: " + --_hp;
    base.MoveOrAttack<T>(x, y);
    if (Move(x, y, out RaycastHit2D hit)) {
      SoundManager.Instance.PlayRandomClip(_moveSound1, _moveSound2);
    }
    CheckForGameOver();
    GameManager.Instance.PlayersTurn = false;
  }

  protected override void Attack<T>(T component) {
    Wall wall = component as Wall;

    wall.DamageWall(_attackPower);
    _animator.SetTrigger("playerAttack");
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Exit")) {
      GameManager.Instance.LoadNextLevel();
    } else if (other.CompareTag("Food")) {
      _hp += _hpPerFood;
      _hpText.text = "+" + _hpPerFood + " HP: " + _hp;
      SoundManager.Instance.PlayRandomClip(_eatSound1, _eatSound2);
      Destroy(other.gameObject);
    } else if (other.CompareTag("Soda")) {
      _hp += _hpPerSoda;
      _hpText.text = "+" + _hpPerSoda + " HP: " + _hp;
      SoundManager.Instance.PlayRandomClip(_drinkSound1, _drinkSound2);
      Destroy(other.gameObject);
    }
  }

  private void CheckForGameOver() {
    if (_hp <= 0) {
      gameObject.SetActive(false);
      SoundManager.Instance.MusicSource.Stop();
      SoundManager.Instance.PlayClip(_gameOverSound);
      GameManager.Instance.GameOver();
    }
  }
}
