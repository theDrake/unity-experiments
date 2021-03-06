﻿using UnityEngine;

public class DestroyByContact : MonoBehaviour {
  public GameObject explosion, playerExplosion;
  public int scoreValue;

  private GameController gameController;
  private Transform thisTransform, otherTransform;

  void Start() {
    GameObject gameControllerObject = GameObject.FindWithTag("GameController");

    thisTransform = GetComponent<Transform>();
    if (gameControllerObject) {
      gameController = gameControllerObject.GetComponent<GameController>();
    }
    if (!gameController) {
      Debug.Log("Cannot find 'GameController' script");
    }
  }

  void OnTriggerEnter(Collider other) {
    if (other.CompareTag("Boundary") || other.CompareTag("Enemy")) {
      return;
    }
    Destroy(gameObject);
    Destroy(other.gameObject);
    if (explosion != null) {
      Instantiate(explosion, thisTransform.position, thisTransform.rotation);
    }
    if (other.CompareTag("Player")) {
      otherTransform = other.GetComponent<Transform>();
      Instantiate(playerExplosion, otherTransform.position,
                  otherTransform.rotation);
      gameController.GameOver();
    }
    gameController.AddToScore(scoreValue);
  }
}
