using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {
  private readonly float[] _spawnDelayValues = {
    1.5f, // easy
    1.0f, // medium
    0.7f, // hard
  };

  public List<Junk> junk;
  public GameObject titleScreen;
  public GameObject pauseScreen;
  public GameObject gameOverScreen;
  public bool playing;
  public bool paused;

  private TextMeshProUGUI _scoreText;
  private TextMeshProUGUI _livesText;
  private AudioSource _music;
  private Slider _musicSlider;
  private float _spawnDelay;
  private int _score;
  private int _lives;

  void Start() {
    titleScreen.SetActive(true);
    pauseScreen.SetActive(false);
    gameOverScreen.SetActive(false);
    _scoreText = GameObject.Find("Score Text").GetComponent<TextMeshProUGUI>();
    _livesText = GameObject.Find("Lives Text").GetComponent<TextMeshProUGUI>();
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

  void Update() {
    if (playing && Input.GetKeyDown(KeyCode.Escape)) {
      if (paused) {
        paused = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1.0f;
      } else {
        Time.timeScale = 0;
        paused = true;
        pauseScreen.SetActive(true);
      }
    }
  }

  public void StartGame() {
    playing = true;
    paused = false;
    _score = 0;
    UpdateScore(0);
    _lives = 3;
    UpdateLives(0);
    titleScreen.SetActive(false);
    StartCoroutine(SpawnJunk());
  }

  private void GameOver() {
    playing = false;
    gameOverScreen.SetActive(true);
    Junk[] junk = FindObjectsOfType<Junk>();
    foreach (Junk j in junk) {
      j.Explode();
    }
  }

  public void RestartGame() {
    // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    gameOverScreen.SetActive(false);
    titleScreen.SetActive(true);
  }

  private IEnumerator SpawnJunk() {
    while (playing) {
      Instantiate(junk[Random.Range(0, junk.Count)]);
      yield return new WaitForSeconds(_spawnDelay);
    }
  }

  public void SetMusicVolume(float sliderValue) {
    _music.volume = sliderValue;
  }

  public void SetDifficulty(int d) {
    _spawnDelay = _spawnDelayValues[d];
    StartGame();
  }

  public void UpdateScore(int points) {
    _score += points;
    _scoreText.text = "Score: " + _score;
  }

  public void UpdateLives(int change) {
    _lives += change;
    _livesText.text = "Lives: " + _lives;
    if (_lives <= 0) {
      GameOver();
    }
  }
}
