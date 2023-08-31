using UnityEngine;

public class BattleEnemy : MonoBehaviour {
  public float speed;

  private Rigidbody _rb;
  private GameObject _target;
  private const float _boundary = 35.0f;

  private void Start() {
    _rb = GetComponent<Rigidbody>();
    _target = GameObject.Find("Player");
  }

  private void FixedUpdate() {
    if (Grounded()) {
      if (_target.transform.position.y > 0) {
        _rb.AddForce((_target.transform.position -
                      transform.position).normalized * speed);
      } else {
        _rb.AddForce((speed / 2) *
                     (Vector3.zero - transform.position).normalized);
      }
    } else if (Vector3.Distance(transform.position, Vector3.zero) > _boundary) {
      Destroy(gameObject);
    }
  }

  private bool Grounded() {
    return Physics.Raycast(transform.position, Vector3.down,
                           GetComponent<Collider>().bounds.extents.y + 0.1f);
  }
}
