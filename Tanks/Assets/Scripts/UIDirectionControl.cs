using UnityEngine;

public class UIDirectionControl : MonoBehaviour {
  private const bool _useRelativeRotation = true;

  private Quaternion _relativeRotation;

  private void Start() {
    _relativeRotation = transform.parent.localRotation;
  }

  private void Update() {
    if (_useRelativeRotation) {
      transform.rotation = _relativeRotation;
    }
  }
}
