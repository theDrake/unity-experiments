using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
  public static GameManager instance = null;
  public float levelStartDelay = 2.0f;
  public float turnDelay = 0.1f;
  public int playerHp = 100;
  [HideInInspector] public bool playersTurn = true;

  private bool initializing = true;
  private int level = 1;
  private Text levelText;
  private GameObject levelImage;
  private LevelManager levelManager;
  private List<Enemy> enemies;
  private bool enemiesMoving;

  void Awake() {
    if (instance == null) {
      instance = this;
    } else if (instance != this) {
      Destroy(gameObject);
    }
    DontDestroyOnLoad(gameObject);
    enemies = new List<Enemy>();
    levelManager = GetComponent<LevelManager>();
    InitializeGame();
  }

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
  static public void CallbackInitialization() {
      SceneManager.sceneLoaded += OnSceneLoaded;
  }

  static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) {
      instance.level++;
      instance.InitializeGame();
  }

  void InitializeGame() {
    initializing = true; // prevents player from moving
    levelImage = GameObject.Find("LevelImage");
    levelText = GameObject.Find("LevelText").GetComponent<Text>();
    levelText.text = "Day " + level;
    levelImage.SetActive(true);
    Invoke("HideLevelImage", levelStartDelay);
    enemies.Clear();
    levelManager.InitializeLevel(level);
  }

  void HideLevelImage() {
    levelImage.SetActive(false);
    initializing = false; // player may now move
  }

  void Update() {
    if (!playersTurn && !enemiesMoving && !initializing) {
      StartCoroutine(MoveEnemies());
    }
  }

  public void AddEnemy(Enemy enemy) {
    enemies.Add(enemy);
  }

  public void GameOver() {
    levelText.text = "After " + level + " day";
    if (level > 1) {
      levelText.text += "s";
    }
    levelText.text += ", you have fallen.";
    levelImage.SetActive(true);
    enabled = false;
  }

  IEnumerator MoveEnemies() {
    enemiesMoving = true;
    yield return new WaitForSeconds(turnDelay);
    if (enemies.Count == 0) {
      yield return new WaitForSeconds(turnDelay);
    }
    for (int i = 0; i < enemies.Count; ++i) {
      enemies[i].Move();
      yield return new WaitForSeconds(enemies[i].moveTime);
    }
    playersTurn = true;
    enemiesMoving = false;
  }
}
