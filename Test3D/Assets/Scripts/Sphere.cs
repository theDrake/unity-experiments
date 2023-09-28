using System.Collections;
using UnityEngine;

public class Sphere : MonoBehaviour {
  private const float _colorShiftRate = 2.0f;
  private const float _bounceForce = 500.0f;

  private Rigidbody _rb;
  private Material _material;
  private Color _currentColor;
  private Color _targetColor;

  private void Start() {
    _rb = GetComponent<Rigidbody>();
    _material = GetComponent<MeshRenderer>().material;
    _material.color = Random.ColorHSV();
    SetNewTargetColor();
    StartCoroutine(ShiftColor());
    InvokeRepeating(nameof(SetNewTargetColor), 0, _colorShiftRate);
  }

  private void OnCollisionEnter(Collision c) {
    if (c.gameObject.CompareTag("Hot")) {
      _rb.AddForce(Vector3.up * _bounceForce);
    }
  }

  private void SetNewTargetColor() {
    _currentColor = _material.color;
    _targetColor = Random.ColorHSV();
  }

  private IEnumerator ShiftColor() {
    while (_material.color != _targetColor) {
      _material.color = Color.Lerp(_currentColor, _targetColor,
                                   Mathf.PingPong(Time.time, 1.0f));
      yield return null;
    }
  }
}
