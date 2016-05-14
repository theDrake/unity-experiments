using UnityEngine;
using UnityEngine.EventSystems;

public class Touchpad : MonoBehaviour, IPointerDownHandler, IDragHandler,
    IPointerUpHandler {
  public float smoothing;

  private bool touched;
  private int pointerID;
  private Vector2 origin, direction, smoothDirection;

  void Awake() {
    touched = false;
    direction = Vector2.zero;
  }

  public void OnPointerDown(PointerEventData data) {
    if (!touched) {
      touched = true;
      pointerID = data.pointerId;
      origin = data.position;
    }
  }

  public void OnDrag(PointerEventData data) {
    if (data.pointerId == pointerID) {
      Vector2 currentPosition = data.position,
              directionRaw = currentPosition - origin;
      direction = directionRaw.normalized;
    }
  }

  public void OnPointerUp(PointerEventData data) {
    if (data.pointerId == pointerID) {
      direction = Vector2.zero;
      touched = false;
    }
  }

  public Vector2 GetDirection() {
    smoothDirection = Vector2.MoveTowards(smoothDirection, direction,
                                          smoothing);

    return smoothDirection;
  }
}
