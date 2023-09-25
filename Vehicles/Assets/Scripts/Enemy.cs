using System.Collections.Generic;
using UnityEngine;

public class Enemy : GameCharacter {
  protected List<Vehicle> _potentialTargets;
  protected Vehicle _target;

  public void SetPotentialTargets(Vehicle[] vehicles) {
    _potentialTargets = new();
    foreach (Vehicle v in vehicles) {
      if (v.gameObject != _vehicle.gameObject) {
        _potentialTargets.Add(v);
      }
    }
  }

  protected virtual void FixedUpdate() {
    if (!Alive()) {
      return;
    } else if (_target && _target.gameObject.activeSelf &&
               Random.Range(0, 500) > 0) {
      _vehicle.MoveToward(_target.transform.position);
    } else {
      _vehicle.Move(0, 0);
      FindNewTarget();
    }
  }

  public virtual void FindNewTarget() {
    GameManager.ShuffleList(_potentialTargets);
    while (_potentialTargets.Count > 0) {
      if (_potentialTargets[0] &&
          _potentialTargets[0].gameObject.activeSelf) {
        _target = _potentialTargets[0];
        return;
      } else {
        _potentialTargets.RemoveAt(0);
      }
    }
  }
}
