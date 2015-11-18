using UnityEngine;
using UnityEngine.EventSystems;

public class Touchpad : MonoBehaviour, IPointerDownHandler, IDragHandler,
    IPointerUpHandler {

  private Vector2 origin, direction;

  void Awake() {
    direction = Vector2.zero;
  }

  public void OnPointerDown(PointerEventData data) {
    origin = data.position;
  }

  public void OnDrag(PointerEventData data) {
    Vector2 currentPosition = data.position,
            directionRaw = currentPosition - origin;

    direction = directionRaw.normalized;
  }

  public void OnPointerUp(PointerEventData data) {
    direction = Vector2.zero;
  }

  public Vector2 GetDirection() {
    return direction;
  }
}
