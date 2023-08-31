using UnityEngine;

public class BattleCharge : MonoBehaviour {
  private const float _chargeForce = 5f;

  private void OnTriggerStay(Collider other) {
    if (other.CompareTag("Enemy")) {
      Rigidbody enemyRb = other.GetComponent<Rigidbody>();
      enemyRb.AddForce(Vector3.up * _chargeForce, ForceMode.Impulse);
    }
  }
}
