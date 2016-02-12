using UnityEngine;
using System.Collections;

public class SetParticleSortingLayer : MonoBehaviour {
  public string sortingLayerName;

  void Start() {
    GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName =
      sortingLayerName;
  }
}
