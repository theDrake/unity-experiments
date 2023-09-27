using UnityEngine;

public class Ball : MonoBehaviour {
  private Vector3 _scaleModifier = new(0.0001f, 0.0001f, 0.0001f);

  private void Update() {
    transform.localScale += _scaleModifier;
  }
}
