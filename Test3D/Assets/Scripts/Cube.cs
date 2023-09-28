using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour {
  private const float _rotationRate = 300.0f;
  private const float _colorShiftRate = 2.0f;

  private Material _material;
  private Color _currentColor;
  private Color _targetColor;

  private void Start() {
    _material = GetComponent<MeshRenderer>().material;
    _material.color = Random.ColorHSV();
    SetNewTargetColor();
    StartCoroutine(ShiftCubeColor());
    InvokeRepeating(nameof(SetNewTargetColor), 0, _colorShiftRate);
  }

  private void Update() {
    transform.Rotate(Vector3.up,
        _rotationRate * Input.GetAxis("Vertical") * Time.deltaTime);
    transform.Rotate(Vector3.right,
        _rotationRate * Input.GetAxis("Horizontal") * Time.deltaTime);
  }

  private void SetNewTargetColor() {
    _currentColor = _material.color;
    _targetColor = Random.ColorHSV();
  }

  private IEnumerator ShiftCubeColor() {
    while (_material.color != _targetColor) {
      _material.color = Color.Lerp(_currentColor, _targetColor,
                                   Mathf.PingPong(Time.time, 1.0f));
      yield return null;
    }
  }
}
