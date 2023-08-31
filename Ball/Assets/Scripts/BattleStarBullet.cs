using UnityEngine;

public class BattleStarBullet : MonoBehaviour {
  private const float _speed = 0.6f;
  private const float _starForce = 30.0f;
  private const float _boundary = 20.0f;
  private GameObject _player;
  private BattleEnemy[] _enemies;
  private BattleEnemy _target = null;

  private void Start() {
    _player = GameObject.FindWithTag("Player");
  }

  private void Update() {
    if (_target == null) {
      _enemies = FindObjectsByType<BattleEnemy>(FindObjectsSortMode.None);
      if (_enemies.Length > 0) {
        _target = _enemies[Random.Range(0, _enemies.Length)];
      }
    }
    if (_target != null) {
      transform.Translate((_target.transform.position -
                           transform.position).normalized * _speed);
    } else if (transform.position.magnitude > _boundary) {
      Destroy(gameObject);
    } else {
      transform.Translate(transform.forward * _speed);
    }
  }

  private void OnTriggerEnter(Collider other) {
    if (other.CompareTag("Enemy")) {
      Rigidbody enemyRb = other.GetComponent<Rigidbody>();
      Vector3 awayFromPlayer = (enemyRb.position -
                                _player.transform.position).normalized;
      enemyRb.AddForce(awayFromPlayer * _starForce, ForceMode.Impulse);
      // Vector3 awayFromStar = (enemyRb.position -
      //                         transform.position).normalized;
      // enemyRb.AddForce(awayFromStar * _starForce, ForceMode.Impulse);
      Destroy(gameObject);
    }
  }
}
