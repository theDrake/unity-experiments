using UnityEngine;

public class Player : GameCharacter {
  private void FixedUpdate() {
    if (Alive()) {
      _vehicle.Move(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
    }
  }
}
