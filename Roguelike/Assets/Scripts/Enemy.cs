using UnityEngine;

public class Enemy : GameCharacter {
  [SerializeField] private AudioClip _attackSound1;
  [SerializeField] private AudioClip _attackSound2;
  private Transform _target;
  private bool _skipMove;

  protected override void Start() {
    GameManager.Instance.AddEnemy(this);
    _animator = GetComponent<Animator>();
    _target = GameObject.FindWithTag("Player").transform;
    base.Start();
  }

  public void SeekTarget() {
    int x = 0, y = 0;

    if (Mathf.Abs(_target.position.x - transform.position.x) < float.Epsilon) {
      y = _target.position.y > transform.position.y ? 1 : -1;
    } else {
      x = _target.position.x > transform.position.x ? 1 : -1;
    }
    MoveOrAttack<Player>(x, y);
  }

  protected override void MoveOrAttack<T>(int x, int y) {
    if (_skipMove) {
      _skipMove = false;
    } else {
      base.MoveOrAttack<T>(x, y);
      _skipMove = true;
    }
  }

  protected override void Attack<T>(T component) {
    Player player = component as Player;

    _animator.SetTrigger("enemyAttack");
    SoundManager.Instance.PlayRandomClip(_attackSound1, _attackSound2);
    player.TakeDamage(_attackPower);
  }
}
