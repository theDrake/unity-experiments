using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour
{
  public GameObject explosion, playerExplosion;
  public int scoreValue;

  private GameController gameController;
  private Transform thisTransform, otherTransform;

  void Start()
  {
    thisTransform = GetComponent<Transform>();
    GameObject gameControllerObject = GameObject.FindWithTag("GameController");
    if (gameControllerObject)
    {
      gameController = gameControllerObject.GetComponent<GameController>();
    }
    if (!gameController)
    {
      Debug.Log("Cannot find 'GameController' script");
    }
  }

  void OnTriggerEnter(Collider other)
  {
    if (other.tag == "Boundary")
      return;
    Destroy(gameObject);
    Destroy(other.gameObject);
    Instantiate(explosion, thisTransform.position, thisTransform.rotation);
    if (other.tag == "Player")
    {
      otherTransform = other.GetComponent<Transform>();
      Instantiate(playerExplosion,
                  otherTransform.position,
                  otherTransform.rotation);
      gameController.GameOver();
    }
    gameController.AddToScore(scoreValue);
  }
}
