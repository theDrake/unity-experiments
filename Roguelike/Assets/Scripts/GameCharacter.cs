using System.Collections;
using UnityEngine;

public abstract class GameCharacter : MonoBehaviour {
  public const float MoveDuration = 0.1f;

  protected Animator _animator;
  [SerializeField] protected int _attackPower;

  [SerializeField] private LayerMask _blockingLayer;
  private BoxCollider2D _collider;
  private float _inverseMoveDuration;
  private bool _moving;

  protected virtual void Start() {
    _collider = GetComponent<BoxCollider2D>();
    _inverseMoveDuration = 1.0f / MoveDuration;
  }

  protected abstract void Attack<T>(T component) where T : Component;

  protected virtual bool MoveOrAttack<T>(int x, int y) where T : Component {
    bool canMove = Move(x, y, out RaycastHit2D hit);

    if (hit.transform) {
      hit.transform.TryGetComponent<T>(out T hitComponent);
      if (!canMove && hitComponent) {
        Attack(hitComponent);

        return true;
      }
    }

    return canMove;
  }

  protected virtual bool Move(int x, int y, out RaycastHit2D hit) {
    Vector2 start = transform.position;
    Vector2 end = start + new Vector2(x, y);

    _collider.enabled = false;
    hit = Physics2D.Linecast(start, end, _blockingLayer);
    _collider.enabled = true;
    if (!hit.transform && !_moving) {
      StartCoroutine(SmoothMovement(end));

      return true;
    }

    return false;
  }

  protected IEnumerator SmoothMovement(Vector3 end) {
    float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

    _moving = true;
    while (sqrRemainingDistance > float.Epsilon) {
      transform.position = Vector3.MoveTowards(transform.position, end,
          _inverseMoveDuration * Time.deltaTime);
      sqrRemainingDistance = (transform.position - end).sqrMagnitude;

      yield return null;
    }
    transform.position = end;
    _moving = false;
  }
}
