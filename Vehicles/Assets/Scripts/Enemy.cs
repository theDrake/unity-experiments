using UnityEngine;

public class Enemy : GameCharacter {
  protected Vehicle _target;
  protected Camera _camera;

  protected override void Start() {
    _camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    base.Start();
  }

  protected virtual void FixedUpdate() {
    if (Dead()) {
      return;
    } else if (_target && _target.Health > 0) {
      _vehicle.MoveToward(_target.transform.position);
    } else {
      FindNewTarget();
    }
    _vehicle.SetHealthBarRotation(_camera.transform.rotation);
  }

  public virtual void FindNewTarget() {
    Vehicle[] vehicles = FindObjectsByType<Vehicle>(FindObjectsSortMode.None);
    int i = Random.Range(0, vehicles.Length);

    while (vehicles[i] == _vehicle && vehicles.Length > 1) {
      i = Random.Range(0, vehicles.Length);
    }
    _target = vehicles[i];
  }
}
