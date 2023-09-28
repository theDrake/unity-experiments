using UnityEngine;

public class MoveForward : MonoBehaviour {
  public float speed;

  private void Update() {
    transform.Translate(speed * Time.deltaTime * Vector3.forward);
  }
}
