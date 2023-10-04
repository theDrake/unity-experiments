using UnityEngine;

public class DestroyByContact : MonoBehaviour {
  public GameObject explosion, playerExplosion;
  public int scoreValue;

  private GameManager _gameManager;

  private void Start() {
    _gameManager = FindAnyObjectByType<GameManager>();
  }

  private void OnTriggerEnter(Collider other) {
    if (other.CompareTag("Boundary") || other.CompareTag("Enemy")) {
      return;
    }
    Destroy(gameObject);
    if (explosion) {
      Instantiate(explosion, transform.position, transform.rotation);
    }
    if (other.CompareTag("Player")) {
      other.gameObject.SetActive(false);
      Instantiate(playerExplosion, other.transform.position,
                  other.transform.rotation);
      _gameManager.GameOver();
    } else {
      Destroy(other.gameObject);
    }
    _gameManager.AddToScore(scoreValue);
  }
}
