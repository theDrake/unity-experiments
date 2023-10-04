﻿using UnityEngine;

public class DestroyByBoundary : MonoBehaviour {
  private void OnTriggerExit(Collider other) {
    Destroy(other.gameObject);
  }
}
