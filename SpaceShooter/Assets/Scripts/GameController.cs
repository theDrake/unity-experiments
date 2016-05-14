using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {
  public int hazardCount;
  public float startWait, spawnWait, waveWait;
  public GameObject[] hazards;
  public GameObject restartButton;
  public Vector3 spawnValues;
  public GUIText scoreText, restartText, gameOverText;

  private bool gameOver;
  private int score;

  void Start() {
    score = 0;
    gameOver = false;
    restartText.text = "";
    gameOverText.text = "";
    restartButton.SetActive(false);
    UpdateScoreText();
    StartCoroutine(SpawnWaves());
  }

  //void Update() {
  //  if (gameOver && Input.GetKeyDown(KeyCode.R)) {
  //    RestartGame();
  //  }
  //}

  public void GameOver() {
    gameOver = true;
    gameOverText.text = "Game Over";
    //restartText.text = "Press 'R' to Restart";
    restartButton.SetActive(true);
  }

  public void AddToScore(int value) {
    score += value;
    UpdateScoreText();
  }

  void UpdateScoreText() {
    scoreText.text = "Score: " + score;
  }

  public void RestartGame() {
    Application.LoadLevel(Application.loadedLevel);
  }

  IEnumerator SpawnWaves() {
    yield return new WaitForSeconds(startWait);
    while (!gameOver) {
      for (int i = 0; i < hazardCount && !gameOver; i++) {
        GameObject hazard = hazards[Random.Range(0, hazards.Length)];
        Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x,
                                                         spawnValues.x),
                                            spawnValues.y,
                                            spawnValues.z);
        Quaternion spawnRotation = Quaternion.identity;
        Instantiate(hazard, spawnPosition, spawnRotation);
        yield return new WaitForSeconds(spawnWait);
      }
      yield return new WaitForSeconds(waveWait);
    }
  }
}
