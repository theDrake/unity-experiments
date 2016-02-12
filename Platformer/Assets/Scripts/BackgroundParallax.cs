﻿using UnityEngine;
using System.Collections;

public class BackgroundParallax : MonoBehaviour {
  public Transform[] backgrounds;
  public float parallaxScale;
  public float parallaxReductionFactor;
  public float smoothing;

  private Transform cam;
  private Vector3 previousCamPos;

  void Awake() {
    cam = Camera.main.transform;
  }

  void Start() {
    previousCamPos = cam.position;
  }

  void Update() {
    float parallax = (previousCamPos.x - cam.position.x) * parallaxScale;
    for (int i = 0; i < backgrounds.Length; i++) {
      float backgroundTargetPosX = backgrounds[i].position.x + parallax *
        (i * parallaxReductionFactor + 1);
      Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX,
                                                backgrounds[i].position.y,
                                                backgrounds[i].position.z);
      backgrounds[i].position = Vector3.Lerp(backgrounds[i].position,
                                             backgroundTargetPos,
                                             smoothing * Time.deltaTime);
    }
    previousCamPos = cam.position;
  }
}
