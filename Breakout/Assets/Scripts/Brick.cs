using UnityEngine;
using UnityEngine.Events;

public class Brick : MonoBehaviour {
  public UnityEvent<int> onDestroyed;
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
    onDestroyed.Invoke(PointValue);

    // slight delay to ensure ball has time to bounce
    Destroy(gameObject, 0.2f);
  }
}
