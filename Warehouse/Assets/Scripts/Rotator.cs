using UnityEngine;

public class Rotator : MonoBehaviour {
  private readonly Vector3 _rotationSpeed = new(0, 200.0f, 0);

  private void Update () {
    transform.Rotate(_rotationSpeed * Time.deltaTime);
  }
}
