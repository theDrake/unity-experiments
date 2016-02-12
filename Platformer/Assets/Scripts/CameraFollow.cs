﻿using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
  public float xMargin = 1f;  // Movement along x-axis before camera follows.
  public float yMargin = 1f;  // Movement along y-axis before camera follows.
  public float xSmooth = 8f;
  public float ySmooth = 8f;
  public Vector2 maxXAndY;
  public Vector2 minXAndY;

  private Transform player;

  void Awake() {
    player = GameObject.FindGameObjectWithTag("Player").transform;
  }

  bool CheckXMargin() {
    return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
  }

  bool CheckYMargin() {
    return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
  }

  void FixedUpdate() {
    TrackPlayer();
  }

  void TrackPlayer() {
    // By default, target coordinates of camera are it's current coordinates.
    float targetX = transform.position.x;
    float targetY = transform.position.y;

    if (CheckXMargin()) {
      targetX = Mathf.Lerp(transform.position.x, player.position.x,
                           xSmooth * Time.deltaTime);
    }
    if (CheckYMargin()) {
      targetY = Mathf.Lerp(transform.position.y, player.position.y,
                           ySmooth * Time.deltaTime);
    }
    targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
    targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);
    transform.position = new Vector3(targetX, targetY, transform.position.z);
  }
}
