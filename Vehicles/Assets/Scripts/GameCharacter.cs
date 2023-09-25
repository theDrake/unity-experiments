using UnityEngine;

public class GameCharacter : MonoBehaviour {
  protected Vehicle _vehicle;

  protected virtual void Awake() {
    _vehicle = GetComponent<Vehicle>();
  }

  public virtual bool Alive() {
    return _vehicle && _vehicle.gameObject.activeSelf;
  }
}
