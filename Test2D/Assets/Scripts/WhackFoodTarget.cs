using System.Collections;
using UnityEngine;

public class WhackFoodTarget : MonoBehaviour {
  private const float _timeOnScreen = 1.0f; // seconds

  private WhackFoodManager _gameManager;
  [SerializeField] private GameObject _explosionFx;
  private Rigidbody _rb;
  [SerializeField] private int _pointValue;

  private void Start() {
    _rb = GetComponent<Rigidbody>();
    _gameManager = FindAnyObjectByType<WhackFoodManager>();
    StartCoroutine(RemovalRoutine());
  }

  public void Explode () {
    Destroy(gameObject);
    Instantiate(_explosionFx,
                new(transform.position.x, transform.position.y, -5.0f),
                _explosionFx.transform.rotation);
  }

  private void OnMouseDown() {
    if (_gameManager.playing && !_gameManager.paused) {
      _gameManager.UpdateScore(_pointValue);
      Explode();
    }
  }

  private IEnumerator RemovalRoutine () {
    yield return new WaitForSeconds(_timeOnScreen);
    if (_gameManager.playing && !_gameManager.paused) {
      Destroy(gameObject);
      if (!gameObject.CompareTag("Bad")) {
        _gameManager.UpdateLives(-1);
      }
    }
  }
}
