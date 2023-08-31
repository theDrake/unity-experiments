using UnityEngine;

public class BattleShield : MonoBehaviour {
  private const float _shieldForce = 10.0f;

  private void OnTriggerStay(Collider other) {
    if (other.CompareTag("Enemy")) {
      Rigidbody enemyRb = other.GetComponent<Rigidbody>();
      Vector3 away = (enemyRb.position - transform.position).normalized;
      enemyRb.AddForce(away * _shieldForce, ForceMode.Impulse);
    }
  }
}
