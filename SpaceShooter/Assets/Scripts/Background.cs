using UnityEngine;

public class Background : MonoBehaviour {
  private const float _scrollSpeed = -0.25f;
  private const float _tileSizeZ = 30.0f;

  private Vector3 _startPosition;

  private void Start() {
    _startPosition = transform.position;
  }

  private void Update() {
    transform.position = _startPosition + Vector3.forward *
        Mathf.Repeat(Time.time * _scrollSpeed, _tileSizeZ);
  }
}
