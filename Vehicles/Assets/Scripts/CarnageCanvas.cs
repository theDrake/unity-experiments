using TMPro;
using UnityEngine;

public class CarnageCanvas : MonoBehaviour {
  private static TextMeshProUGUI _speedText;
  private static TextMeshProUGUI _enemiesText;
  private static GameObject _victoryText;
  private static GameObject _gameOverText;
  private static Vehicle _player;
  private static int _numEnemies;

  private void Start() {
    _player = FindAnyObjectByType<Player>().GetComponent<Vehicle>();
    _victoryText = GameObject.Find("Victory Text");
    _gameOverText = GameObject.Find("Game Over Text");
    _victoryText.SetActive(false);
    _gameOverText.SetActive(false);
    _speedText = GameObject.Find("Speed Text").GetComponent<TextMeshProUGUI>();
    _enemiesText = GameObject.Find(
        "Enemies Text").GetComponent<TextMeshProUGUI>();
    SetNumEnemies(0);
  }

  private void Update() {
    float speed = _player ? _player.GetSpeed() : 0;

    _speedText.text = "Speed: " + GetMph(speed) + " mph / " + GetKph(speed) +
        " kph";
  }

  public static void SetNumEnemies(int n) {
    _numEnemies = n;
    _enemiesText.text = "Enemies: " + _numEnemies;
    _victoryText.SetActive(false);
  }

  public static void DecrementNumEnemies() {
    SetNumEnemies(_numEnemies - 1);
    if (_numEnemies <= 0 && CarnageManager.Instance.Victorious()) {
      _victoryText.SetActive(true);
    }
  }

  public static void ShowGameOver() {
    _gameOverText.SetActive(true);
  }

  private float GetMph(float n) {
    return Mathf.Round(n * 2.237f);
  }

  private float GetKph(float n) {
    return Mathf.Round(n * 3.6f);
  }
}
