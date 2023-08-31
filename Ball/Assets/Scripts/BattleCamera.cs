using UnityEngine;

public class BattleCamera : MonoBehaviour {
  private const float _speed = 50;

  private void Update() {
    transform.Rotate(Vector3.up,
                     Input.GetAxis("Mouse X") * _speed * Time.deltaTime);
  }
}
