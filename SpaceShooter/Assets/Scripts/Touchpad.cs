using UnityEngine;
using UnityEngine.EventSystems;

public class Touchpad : MonoBehaviour, IPointerDownHandler, IDragHandler,
                        IPointerUpHandler {
  private const float _smoothing = 0.1f;

  private Vector2 _origin;
  private Vector2 _direction;
  private Vector2 _smoothDirection;
  private int _pointerId;
  private bool _touched;

  public void OnPointerDown(PointerEventData data) {
    if (!_touched) {
      _touched = true;
      _pointerId = data.pointerId;
      _origin = data.position;
    }
  }

  public void OnDrag(PointerEventData data) {
    if (data.pointerId == _pointerId) {
      _direction = (data.position - _origin).normalized;
    }
  }

  public void OnPointerUp(PointerEventData data) {
    if (data.pointerId == _pointerId) {
      _direction = Vector2.zero;
      _touched = false;
    }
  }

  public Vector2 GetDirection() {
    return _smoothDirection = Vector2.MoveTowards(_smoothDirection, _direction,
                                                  _smoothing);
  }
}
