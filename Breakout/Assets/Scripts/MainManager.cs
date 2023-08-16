using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour {
  public Brick BrickPrefab;
  public int LineCount = 6;
  public Rigidbody Ball;
  public Text ScoreText;
  public Text HighScoreText;
  public GameObject GameOverText;

  [Serializable]
  class HighScoreData {
    public int Score;
    public string Name;
  }

  private bool _started = false;
  private bool _gameOver = false;
  private int _score;
  private int _numBricks;
  private int _highScore;
  private string _username;
  private string _highScoreName;
  private string _highScoreDataPath;

  private void Start() {
    _highScoreDataPath = Application.persistentDataPath + "/highscore.json";
    LoadHighScore();
    CreateBricks();
    if (StartMenu.Instance) {
      _username = StartMenu.Instance.Username.text;
      StartMenu.Instance.gameObject.SetActive(false);
    } else {
      _username = "";
    }
    UpdateScoreText();
  }

  private void Update() {
    if (Input.GetKeyDown(KeyCode.Escape)) {
      LoadStartMenu();
    } else if (!_started) {
      if (Input.GetKeyDown(KeyCode.Space)) {
        float randomDirection = UnityEngine.Random.Range(-1.0f, 1.0f);
        Vector3 forceDir = new Vector3(randomDirection, 1, 0);
        forceDir.Normalize();
        Ball.transform.SetParent(null);
        Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
        _started = true;
      }
    } else if (_gameOver) {
      if (Input.GetKeyDown(KeyCode.Space)) {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
      }
    }
  }

  private void CreateBricks() {
    const float step = 0.6f;
    int perLine = Mathf.FloorToInt(4.0f / step);
    int[] pointCountArray = new [] {1,1,2,2,5,5};

    _numBricks = 0;
    for (int i = 0; i < LineCount; ++i) {
      for (int x = 0; x < perLine; ++x) {
        Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
        var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
        brick.PointValue = pointCountArray[i];
        brick.OnDestroyed.AddListener(AddPoints);
        ++_numBricks;
      }
    }
  }

  private void AddPoints(int points) {
    _score += points;
    UpdateScoreText();
    if (--_numBricks <= 0) {
      GameOver();
    }
  }

  private void UpdateScoreText() {
    ScoreText.text = $"Score: {_score}\t\tName: {_username}";
  }

  private void SaveHighScore() {
    HighScoreData data = new HighScoreData();
    data.Score = _highScore;
    data.Name = _highScoreName;
    string json = JsonUtility.ToJson(data);
    File.WriteAllText(_highScoreDataPath, json);
  }

  private void LoadHighScore() {
    if (File.Exists(_highScoreDataPath)) {
      string json = File.ReadAllText(_highScoreDataPath);
      HighScoreData data = JsonUtility.FromJson<HighScoreData>(json);
      _highScore = data.Score;
      _highScoreName = data.Name;
      UpdateHighScoreText();
    } else {
      _highScore = 0;
      _highScoreName = "";
    }
  }

  private void UpdateHighScoreText() {
    HighScoreText.text = $"High Score: {_highScore}\t\tName: {_highScoreName}";
  }

  public void GameOver() {
    _gameOver = true;
    GameOverText.SetActive(true);
    if (_score > _highScore) {
      _highScore = _score;
      _highScoreName = _username;
      UpdateHighScoreText();
      SaveHighScore();
    }
  }

  private void LoadStartMenu() {
    if (StartMenu.Instance) {
      StartMenu.Instance.gameObject.SetActive(true);
    }
    SceneManager.LoadScene(0);
  }
}
