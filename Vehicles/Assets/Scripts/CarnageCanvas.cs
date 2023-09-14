using TMPro;
using UnityEngine;

public class CarnageCanvas : MonoBehaviour {
  private static TextMeshProUGUI _speedText;
  private static TextMeshProUGUI _enemiesText;
  private static TextMeshProUGUI _gameOverText;
  private static Vehicle _player;
  private static int _numEnemies;

  private void Start() {
    _player = FindAnyObjectByType<Player>().GetComponent<Vehicle>();
    _gameOverText = GameObject.Find(
        "Game Over Text").GetComponent<TextMeshProUGUI>();
    _gameOverText.gameObject.SetActive(false);
    _speedText = GameObject.Find("Speed Text").GetComponent<TextMeshProUGUI>();
    _enemiesText = GameObject.Find(
        "Enemies Text").GetComponent<TextMeshProUGUI>();
  }

  private void Update() {
    float speed = _player ? _player.GetSpeed() : 0;

    _speedText.text = "Speed: " + GetMph(speed) + " mph / " + GetKph(speed) +
        " kph";
  }

  public static void UpdateNumEnemies(int adjustment=0) {
    _numEnemies += adjustment;
    _enemiesText.text = "Enemies: " + _numEnemies;
    if (_numEnemies <= 0 && GameManager.Instance.Victorious()) {
      ShowVictory();
    }
  }

  public static void ShowGameOver() {
    _gameOverText.text = "Game over!";
    _gameOverText.gameObject.SetActive(true);
  }

  public static void ShowVictory() {
    _gameOverText.text = "You win!";
    _gameOverText.gameObject.SetActive(true);
  }

  public void PlayAgain() {
    _numEnemies = 0;
    _gameOverText.gameObject.SetActive(false);
    GameManager.Instance.StartGame();
  }

  public void ReturnToMenu() {
    GameManager.Instance.ReturnToTitleScreen();
  }

  private float GetMph(float n) {
    return Mathf.Round(n * 2.237f);
  }

  private float GetKph(float n) {
    return Mathf.Round(n * 3.6f);
  }
}
