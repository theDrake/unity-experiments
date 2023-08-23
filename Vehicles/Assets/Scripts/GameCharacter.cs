using UnityEngine;

public class GameCharacter : MonoBehaviour {
  protected Vehicle _vehicle;

  public virtual bool Alive() {
    return _vehicle.GetHealth() > 0;
  }

  public virtual bool Dead() {
    return _vehicle.GetHealth() < 0;
  }

  protected virtual void Start() {
    if (!_vehicle) {
      _vehicle = GetComponent<Vehicle>();
    }
  }
}
