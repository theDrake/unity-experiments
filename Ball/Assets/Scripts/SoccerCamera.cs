using UnityEngine;

public class SoccerCamera : MonoBehaviour {
  public GameObject player;

  private const float _speed = 50;

  private void Update() {
    transform.Rotate(Vector3.up,
                     Input.GetAxis("Mouse X") * _speed * Time.deltaTime);
    transform.position = player.transform.position;
  }
}
