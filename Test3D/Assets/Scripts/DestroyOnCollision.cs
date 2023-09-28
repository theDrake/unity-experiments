using UnityEngine;

public class DestroyOnCollision : MonoBehaviour {
  private void OnTriggerEnter(Collider other) {
    Destroy(gameObject);
  }
}
