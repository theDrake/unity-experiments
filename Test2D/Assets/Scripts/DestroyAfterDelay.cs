using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour {
  private const float _delay = 2.0f; // seconds

  private void Start() {
    Destroy(gameObject, _delay);
  }
}
