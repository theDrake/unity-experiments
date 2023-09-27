using UnityEngine;

public class CustomCamera : MonoBehaviour {
  private readonly Vector3 _positionModifier = new(-0.002f, -0.02f);

  private Transform _cameraTarget;

  private void Start() {
    _cameraTarget = FindAnyObjectByType<TrackFinish>().transform;
  }

  private void Update() {
    if (transform.position.y > _cameraTarget.position.y * 5) {
      transform.Translate(_positionModifier);
    }
    transform.LookAt(_cameraTarget);
  }
}
