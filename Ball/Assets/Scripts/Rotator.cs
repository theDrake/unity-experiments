using UnityEngine;

public class Rotator : MonoBehaviour {
  public Vector3 rotationSpeed;

  private void Update () {
    transform.Rotate(rotationSpeed * Time.deltaTime);
  }
}
