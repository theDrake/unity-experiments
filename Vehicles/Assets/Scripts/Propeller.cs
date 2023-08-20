using UnityEngine;

public class Propeller : MonoBehaviour {
  private float _propellerSpeed = 30.0f;

  void Update() {
    transform.Rotate(0, 0, _propellerSpeed);
  }
}
