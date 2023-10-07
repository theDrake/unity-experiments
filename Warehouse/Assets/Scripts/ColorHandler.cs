using UnityEngine;

public class ColorHandler : MonoBehaviour {
  public Renderer TintRenderer;
  public int TintMaterialSlot;

  public void SetColor(Color c) {
    MaterialPropertyBlock property = new();

    property.SetColor("_BaseColor", c);
    TintRenderer.SetPropertyBlock(property, TintMaterialSlot);
  }
}
