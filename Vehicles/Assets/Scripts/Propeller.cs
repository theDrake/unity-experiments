using UnityEngine;

public class Propeller : MonoBehaviour {
  [SerializeField] private float _propellerSpeed = 20.0f;

  void Update() {
    transform.Rotate(0, 0, _propellerSpeed);
  }
}
