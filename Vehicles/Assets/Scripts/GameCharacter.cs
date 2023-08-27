using UnityEngine;

public class GameCharacter : MonoBehaviour {
  protected Vehicle _vehicle;

  protected virtual void Start() {
    _vehicle = GetComponent<Vehicle>();
  }

  public virtual bool Alive() {
    return _vehicle.Health > 0;
  }

  public virtual bool Dead() {
    return _vehicle.Health <= 0;
  }
}
