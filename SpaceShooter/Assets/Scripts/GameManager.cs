using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {
  private readonly Vector3 _spawn = new(6.0f, 0, 16.0f);
  private const float _startDelay = 1.0f;
  private const float _spawnDelay = 0.5f;
  private const float _waveDelay = 1.0f;
  private const int _hazardsPerWave = 10;

  public GameObject[] hazards;
  public static bool Playing { get; private set; }

  private GameObject _restartButton, _gameOverText;
  private TextMeshProUGUI _scoreText;
  private int _score = 0;

  private void Start() {
    Playing = true;
    _restartButton = GameObject.Find("RestartButton");
    _restartButton.SetActive(false);
    _gameOverText = GameObject.Find("GameOverText");
    _gameOverText.gameObject.SetActive(false);
    _scoreText = GameObject.Find(
        "ScoreText").GetComponent<TextMeshProUGUI>();
    StartCoroutine(SpawnWaves());
  }

  public void GameOver() {
    Playing = false;
    _gameOverText.gameObject.SetActive(true);
    _restartButton.SetActive(true);
  }

  public void AddToScore(int value) {
    _score += value;
    UpdateScoreText();
  }

  public void RestartGame() {
    SceneManager.LoadScene("Main");
  }

  private IEnumerator SpawnWaves() {
    yield return new WaitForSeconds(_startDelay);
    while (Playing) {
      for (int i = 0; i < _hazardsPerWave && Playing; ++i) {
        GameObject hazard = hazards[Random.Range(0, hazards.Length)];
        Vector3 spawnPosition = new(Random.Range(-_spawn.x, _spawn.x),
                                    _spawn.y, _spawn.z);
        Quaternion spawnRotation = Quaternion.identity;
        Instantiate(hazard, spawnPosition, spawnRotation);
        yield return new WaitForSeconds(_spawnDelay);
      }
      yield return new WaitForSeconds(_waveDelay);
    }
  }

  private void UpdateScoreText() {
    _scoreText.text = "Score: " + _score;
  }
}
