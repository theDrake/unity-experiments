using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour {
  // ENCAPSULATION
  private Vehicle _vehicle;
  private GameObject _target;
  // private float _forwardSpeed = 20, _turnSpeed = 20;

  void Start() {
    _vehicle = GetComponent<Vehicle>();
  }

  private void FixedUpdate() {
    if (_target) {
      _vehicle.MoveToward(_target.transform.position);
    } else {
      FindNewTarget();
    }
    // transform.Translate(Vector3.forward * Time.deltaTime * _forwardSpeed);
    // transform.LookAt(_target.transform.position);
  }

  // ABSTRACTION
  public void FindNewTarget() {
    Vehicle[] vehicles = FindObjectsByType<Vehicle>(FindObjectsSortMode.None);
    int i = Random.Range(0, vehicles.Length);

    while (vehicles[i] == _vehicle && vehicles.Length > 1) {
      i = Random.Range(0, vehicles.Length);
    }
    _target = vehicles[i].gameObject;
  }
}
