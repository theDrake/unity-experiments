using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour {
  public bool destroyOnAwake;  // Whether to destroy, after a delay, on Awake.
  public float awakeDestroyDelay;  // The delay for destroying it on Awake.
  public bool findChild = false;  // Find a child game object and delete it.
  public string namedChild;  // Name of the child object in Inspector.

  void Awake() {
    if (destroyOnAwake) {
      if (findChild) {
        Destroy(transform.Find(namedChild).gameObject);
      } else {
        Destroy(gameObject, awakeDestroyDelay);
      }
    }
  }

  void DestroyChildGameObject() {
    if (transform.Find(namedChild).gameObject != null) {
      Destroy(transform.Find(namedChild).gameObject);
    }
  }

  void DisableChildGameObject() {
    if (transform.Find(namedChild).gameObject.activeSelf == true) {
      transform.Find(namedChild).gameObject.SetActive(false);
    }
  }

  void DestroyGameObject() {
    Destroy(gameObject);
  }
}
