using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour {
  public Brick BrickPrefab;
  public Rigidbody Ball;
  public Text ScoreText;
  public Text HighScoreText;
  public GameObject GameOverText;

  [System.Serializable]
  private class HighScoreData {
    public int Score;
    public string Name;
  }

  private bool _started = false;
  private bool _gameOver = false;
  private const int _rows = 6;
  private int _score;
  private int _numBricks;
  private int _highScore;
  private string _username;
  private string _highScoreName;
  private string _highScoreFile;

  private void Start() {
    _highScoreFile = Application.persistentDataPath + "/highscore.json";
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
        Vector3 forceDir = new(Random.Range(-1.0f, 1.0f), 1.0f, 0);

        forceDir.Normalize();
        Ball.transform.SetParent(null);
        Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
        _started = true;
      }
    } else if (_gameOver && Input.GetKeyDown(KeyCode.Space)) {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
  }

  private void CreateBricks() {
    const float step = 0.6f;
    int bricksPerRow = Mathf.FloorToInt(4.0f / step);
    int[] pointValues = new [] {1,1,2,2,5,5};

    _numBricks = 0;
    for (int i = 0; i < _rows; ++i) {
      for (int j = 0; j < bricksPerRow; ++j) {
        Vector3 position = new(-1.5f + step * j, 2.5f + i * 0.3f, 0);
        Brick brick = Instantiate(BrickPrefab, position, Quaternion.identity);

        brick.PointValue = pointValues[i];
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
    HighScoreData data = new() {
      Score = _highScore,
      Name = _highScoreName
    };
    string json = JsonUtility.ToJson(data);

    File.WriteAllText(_highScoreFile, json);
  }

  private void LoadHighScore() {
    if (File.Exists(_highScoreFile)) {
      string json = File.ReadAllText(_highScoreFile);
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
