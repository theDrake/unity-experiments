using TMPro;
using UnityEngine;

public class CarnageCanvas : MonoBehaviour {
  public static int Score {
    get {
      return _score;
    }
    set {
      _score = value;
      _scoreText.text = "Score: " + _score;
    }
  }

  private static TextMeshProUGUI _speedText;
  private static TextMeshProUGUI _enemiesText;
  private static TextMeshProUGUI _scoreText;
  private static TextMeshProUGUI _gameOverText;
  private static Vehicle _player;
  private static int _numEnemies;
  private static int _score;

  private void Awake() {
    _score = 0;
    _numEnemies = 0;
    _player = FindAnyObjectByType<Player>().GetComponent<Vehicle>();
    _gameOverText = GameObject.Find(
        "Game Over Text").GetComponent<TextMeshProUGUI>();
    _gameOverText.gameObject.SetActive(false);
    _speedText = GameObject.Find("Speed Text").GetComponent<TextMeshProUGUI>();
    _enemiesText = GameObject.Find(
        "Enemies Text").GetComponent<TextMeshProUGUI>();
    _scoreText = GameObject.Find("Score Text").GetComponent<TextMeshProUGUI>();
  }

  private void Update() {
    float speed = _player ? _player.GetSpeed() : 0;

    _speedText.text = "Speed: " + GetMph(speed) + " mph / " + GetKph(speed) +
        " kph";
  }

  public static void UpdateNumEnemies(int adjustment=0) {
    _numEnemies += adjustment;
    _enemiesText.text = "Enemies: " + _numEnemies;
    if (_numEnemies <= 0 && _player && _player.gameObject.activeSelf &&
        GameManager.Instance.GetNumEnemies() > 0) {
      ShowVictory();
    }
  }

  public static void ShowGameOver() {
    if (!_gameOverText.gameObject.activeSelf) {
      _gameOverText.text = "Game over!";
      _gameOverText.gameObject.SetActive(true);
    }
  }

  public static void ShowVictory() {
    if (!_gameOverText.gameObject.activeSelf) {
      _gameOverText.text = "You win!";
      _gameOverText.gameObject.SetActive(true);
    }
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
