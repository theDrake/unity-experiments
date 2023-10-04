using UnityEngine;
using UnityEngine.EventSystems;

public class FireZone : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
  public bool CanFire { get; private set; }

  private int _pointerId;
  private bool _touched;

  public void OnPointerDown(PointerEventData data) {
    if (!_touched) {
      _touched = true;
      CanFire = true;
      _pointerId = data.pointerId;
    }
  }

  public void OnPointerUp(PointerEventData data) {
    if (data.pointerId == _pointerId) {
      _touched = false;
      CanFire = false;
    }
  }
}
