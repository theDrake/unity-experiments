using UnityEngine;
using UnityEngine.Events;

public class Brick : MonoBehaviour {
  public UnityEvent<int> OnDestroyed;
  public int PointValue;

  void Start() {
    var renderer = GetComponentInChildren<Renderer>();
    MaterialPropertyBlock block = new MaterialPropertyBlock();

    switch (PointValue) {
      case 1 :
        block.SetColor("_BaseColor", Color.green);
        break;
      case 2:
        block.SetColor("_BaseColor", Color.yellow);
        break;
      case 5:
        block.SetColor("_BaseColor", Color.blue);
        break;
      default:
        block.SetColor("_BaseColor", Color.red);
        break;
    }
    renderer.SetPropertyBlock(block);
  }

  private void OnCollisionEnter(Collision other) {
    OnDestroyed.Invoke(PointValue);
    Destroy(gameObject, 0.1f); // slight delay to give ball time to bounce
  }
}
