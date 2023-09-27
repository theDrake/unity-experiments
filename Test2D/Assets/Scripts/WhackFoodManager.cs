using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WhackFoodManager : MonoBehaviour {
  private const float _minX = -3.75f; // center of left-most square
  private const float _minY = -3.75f; // center of bottom-most square
  private const float _distanceBetweenSquares = 2.5f;
  private const float _defaultSpawnRate = 2.5f; // seconds
  private const int _maxTime = 30; // seconds
  private const int _rowLength = 4;

  public GameObject titleScreen, pauseScreen;
  public List<GameObject> targetPrefabs;
  public TextMeshProUGUI scoreText, timeText, livesText, gameOverText;
  public Button restartButton;
  public bool playing, paused;

  private AudioSource _music;
  private Slider _musicSlider;
  private float _spawnRate; // modified by difficulty
  private int _score, _time, _lives;

  private void Start() {
    pauseScreen.SetActive(false);
    _music = GameObject.Find("Music").GetComponent<AudioSource>();
    if (!_music.isPlaying) {
      _music.Play();
    }
    _musicSlider = GameObject.Find("Music Volume").GetComponent<Slider>();
    SetMusicVolume(_musicSlider.value);
    _musicSlider.onValueChanged.AddListener(delegate {
      SetMusicVolume(_musicSlider.value);
    });
  }

  private void Update() {
    if (playing && Input.GetKeyDown(KeyCode.Escape)) {
      if (paused) {
        paused = false;
        pauseScreen.SetActive(false);
      } else {
        paused = true;
        pauseScreen.SetActive(true);
      }
    }
  }

  public void StartGame(int difficulty) {
    titleScreen.SetActive(false);
    playing = true;
    paused = false;
    _spawnRate = _defaultSpawnRate / difficulty;
    _time = _maxTime;
    _lives = 3;
    UpdateLives(0);
    _score = 0;
    UpdateScore(0);
    StartCoroutine(UpdateTime());
    StartCoroutine(SpawnTarget());
  }

  public void RestartGame() {
    // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    gameOverText.gameObject.SetActive(false);
    restartButton.gameObject.SetActive(false);
    titleScreen.SetActive(true);
  }

  public void UpdateScore(int points) {
    _score += points;
    scoreText.text = "Score: " + _score;
  }

  public void UpdateLives(int change) {
    _lives += change;
    livesText.text = "Lives: " + _lives;
    if (_lives <= 0) {
      GameOver();
    }
  }

  public void SetMusicVolume(float sliderValue) {
    _music.volume = sliderValue;
  }

  private IEnumerator UpdateTime() {
    while (playing) {
      if (playing && !paused) {
        timeText.SetText("Time: " + _time);
        if (_time == 0) {
          GameOver();
        }
        --_time;
      }
      yield return new WaitForSeconds(1);
    }
  }

  private IEnumerator SpawnTarget() {
    while (playing) {
      int i = Random.Range(0, targetPrefabs.Count);
      if (playing && !paused) {
        Instantiate(targetPrefabs[i], GetSpawnPoint(),
                    targetPrefabs[i].transform.rotation);
      }
      yield return new WaitForSeconds(_spawnRate);
    }
  }

  private Vector3 GetSpawnPoint() {
    return new(_minX + (RandomSquareIndex() * _distanceBetweenSquares),
               _minY + (RandomSquareIndex() * _distanceBetweenSquares));
  }

  private int RandomSquareIndex() {
    return Random.Range(0, _rowLength);
  }

  private void GameOver() {
    playing = false;
    gameOverText.gameObject.SetActive(true);
    restartButton.gameObject.SetActive(true);
    StopAllCoroutines();
    foreach (WhackFoodTarget target in FindObjectsOfType<WhackFoodTarget>()) {
      target.Explode();
    }
  }
}
