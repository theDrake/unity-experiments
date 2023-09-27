using UnityEngine;

public class TrackFinish : MonoBehaviour {
  private Vector3 _positionModifier = new(0, 0, 0.01f);

  private void Update() {
    transform.position += _positionModifier;
  }
}
