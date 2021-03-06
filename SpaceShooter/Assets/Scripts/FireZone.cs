﻿using UnityEngine;
using UnityEngine.EventSystems;

public class FireZone : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
  private bool touched, canFire;
  private int pointerID;

  void Awake() {
    touched = false;
  }

  public void OnPointerDown(PointerEventData data) {
    if (!touched) {
      touched = true;
      canFire = true;
      pointerID = data.pointerId;
    }
  }

  public void OnPointerUp(PointerEventData data) {
    if (data.pointerId == pointerID) {
      touched = false;
      canFire = false;
    }
  }

  public bool CanFire() {
    return canFire;
  }
}
