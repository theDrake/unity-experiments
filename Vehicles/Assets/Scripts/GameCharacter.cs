using UnityEngine;

public class GameCharacter : MonoBehaviour {
  protected Vehicle _vehicle;

  protected virtual void Start() {
    _vehicle = GetComponent<Vehicle>();
  }

  public virtual bool Alive() {
    return _vehicle && _vehicle.gameObject.activeSelf;
  }

  public virtual bool Dead() {
    return !Alive();
  }
}
