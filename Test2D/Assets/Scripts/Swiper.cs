using UnityEngine;

[RequireComponent(typeof(TrailRenderer), typeof(BoxCollider))]
public class Swiper : MonoBehaviour {
  private const float _mousePositionZ = 10.0f;

  private GameManager _gameManager;
  private Camera _cam;
  private Vector3 _mousePosition;
  private TrailRenderer _trail;
  private BoxCollider _boxCollider;
  private bool _swiping = false;

  private void Awake() {
    _cam = Camera.main;
    _trail = GetComponent<TrailRenderer>();
    _boxCollider = GetComponent<BoxCollider>();
    _trail.enabled = false;
    _boxCollider.enabled = false;
    _gameManager = FindAnyObjectByType<GameManager>();
  }

  private void Update() {
    if (_gameManager.playing && !_gameManager.paused) {
      if (Input.GetMouseButtonDown(0)) {
        _swiping = true;
        UpdateComponents();
      } else if (Input.GetMouseButtonUp(0)) {
        _swiping = false;
        UpdateComponents();
      }
      if (_swiping) {
        UpdateMousePosition();
      }
    }
  }

  private void UpdateMousePosition() {
    _mousePosition = _cam.ScreenToWorldPoint(new(Input.mousePosition.x,
                                                 Input.mousePosition.y,
                                                 _mousePositionZ));
    transform.position = _mousePosition;
  }

  private void UpdateComponents() {
    _trail.enabled = _swiping;
    _boxCollider.enabled = _swiping;
  }

  private void OnCollisionEnter(Collision collision) {
    if(collision.gameObject.TryGetComponent<Junk>(out Junk j)) {
      j.Explode();
    }
  }
}
