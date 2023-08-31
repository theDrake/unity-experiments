using UnityEngine;

public class SoccerEnemy : MonoBehaviour {
  private const float _boostIncrement = 10.0f;
  private const float _boundary = 30.0f;
  private float _speed = 50.0f;
  private Rigidbody _rb;
  private GameObject _playerGoal;
  private Vector3 _center = new(0, 0, 10.0f);

  private void Start() {
    _rb = GetComponent<Rigidbody>();
    _playerGoal = GameObject.Find("Player Goal");
  }

  private void FixedUpdate() {
    if (Vector3.Distance(transform.position, _center) > _boundary) {
      Destroy(gameObject);
    } else {
      _rb.AddForce(_speed * Time.deltaTime *
          (_playerGoal.transform.position - transform.position).normalized);
    }
  }

  public float BoostSpeed(int multiplier) {
    return _speed += _boostIncrement * multiplier;
  }

  private void OnCollisionEnter(Collision other) {
    if (other.gameObject.name == "Enemy Goal" ||
        other.gameObject.name == "Player Goal") {
      --SoccerSpawnManager.enemyCount;
      Destroy(gameObject);
    }
  }
}
