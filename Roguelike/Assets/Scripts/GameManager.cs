using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
  public static GameManager Instance { get; private set; }

  private const float _levelStartDelay = 1.5f;
  private const float _gameRestartDelay = 3.0f;
  private const float _turnDelay = 0.1f;

  [HideInInspector] public bool PlayersTurn;

  private LevelManager _levelManager;
  private Text _levelText;
  private GameObject _levelImage;
  private List<Enemy> _enemies;
  private int _level;
  private bool _enemiesMoving;

  private void Awake() {
    if (!Instance) {
      Instance = this;
      _enemies = new List<Enemy>();
      _levelManager = GetComponent<LevelManager>();
      _levelImage = GameObject.Find("LevelImage");
      _levelText = GameObject.Find("LevelText").GetComponent<Text>();
      LoadNextLevel();
    } else {
      Destroy(gameObject);
    }
  }

  private void Update() {
    if (!PlayersTurn && !_enemiesMoving) {
      StartCoroutine(MoveEnemies());
    }
  }

  public void LoadNextLevel() {
    PlayersTurn = true;
    _levelText.text = "Day " + ++_level;
    _levelImage.SetActive(true);
    Invoke(nameof(HideLevelImage), _levelStartDelay);
    _enemies.Clear();
    _levelManager.InitializeLevel(_level);
  }

  public void GameOver() {
    _levelText.text = "After " + _level + " day";
    if (_level > 1) {
      _levelText.text += "s";
    }
    _levelText.text += ", you have fallen.";
    _levelImage.SetActive(true);
    Invoke(nameof(RestartGame), _gameRestartDelay);
  }

  public void AddEnemy(Enemy enemy) {
    _enemies.Add(enemy);
  }

  private void HideLevelImage() {
    _levelImage.SetActive(false);
  }

  private IEnumerator MoveEnemies() {
    _enemiesMoving = true;
    yield return new WaitForSeconds(_turnDelay);
    if (_enemies.Count == 0) {
      yield return new WaitForSeconds(_turnDelay);
    }
    for (int i = 0; i < _enemies.Count; ++i) {
      _enemies[i].SeekTarget();
      yield return new WaitForSeconds(GameCharacter.MoveDuration);
    }
    PlayersTurn = true;
    _enemiesMoving = false;
  }

  private void RestartGame() {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }
}
