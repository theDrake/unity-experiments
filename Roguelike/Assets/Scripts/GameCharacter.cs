using System.Collections;
using UnityEngine;

public abstract class GameCharacter : MonoBehaviour {
  public const float MoveDuration = 0.1f;

  protected Animator _animator;
  [SerializeField] protected int _attackPower;

  [SerializeField] private LayerMask _blockingLayer;
  private Rigidbody2D _rb;
  private BoxCollider2D _collider;
  private float _inverseMoveDuration;

  protected virtual void Start() {
    _collider = GetComponent<BoxCollider2D>();
    _rb = GetComponent<Rigidbody2D>();
    _inverseMoveDuration = 1.0f / MoveDuration;
  }

  protected bool Move(int x, int y, out RaycastHit2D hit) {
    Vector2 start = transform.position;
    Vector2 end = start + new Vector2(x, y);

    _collider.enabled = false;
    hit = Physics2D.Linecast(start, end, _blockingLayer);
    _collider.enabled = true;
    if (hit.transform == null) {
      StartCoroutine(SmoothMovement(end));

      return true;
    }

    return false;
  }

  protected IEnumerator SmoothMovement(Vector3 end) {
    float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

    while (sqrRemainingDistance > float.Epsilon) {
      _rb.MovePosition(Vector3.MoveTowards(_rb.position, end,
          _inverseMoveDuration * Time.deltaTime));
      sqrRemainingDistance = (transform.position - end).sqrMagnitude;

      yield return null;
    }
  }

  protected virtual void MoveOrAttack<T>(int x, int y) where T : Component {
    bool canMove = Move(x, y, out RaycastHit2D hit);

    if (hit.transform) {
      T hitComponent = hit.transform.GetComponent<T>();
      if (!canMove && hitComponent) {
        Attack(hitComponent);
      }
    }
  }

  protected abstract void Attack<T>(T component) where T : Component;
}
