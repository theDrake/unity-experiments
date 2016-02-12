using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
  public Vector3 offset;  // Offset at which the Health Bar follows the player.

  private Transform player;  // Reference to the player.

  void Awake() {
    player = GameObject.FindGameObjectWithTag("Player").transform;
  }

  void Update() {
    transform.position = player.position + offset;
  }
}
