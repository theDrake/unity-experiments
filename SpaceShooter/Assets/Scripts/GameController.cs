using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
  public GameObject hazard;
  public Vector3 spawnValues;
  public GUIText scoreText, restartText, gameOverText;
  public int hazardCount;
  public float startWait, spawnWait, waveWait;

  private bool gameOver;
  private int score;

  void Start()
  {
    score = 0;
    gameOver = false;
    restartText.text = gameOverText.text = "";
    UpdateScoreText();
    StartCoroutine(SpawnWaves());
  }

  void Update()
  {
    if (gameOver && Input.GetKeyDown(KeyCode.R))
    {
      Application.LoadLevel(Application.loadedLevel);
    }
  }

  public void GameOver()
  {
    gameOver = true;
    gameOverText.text = "Game Over";
    restartText.text = "Press 'R' to Restart";
  }

  public void AddToScore(int value)
  {
    score += value;
    UpdateScoreText();
  }
  
  void UpdateScoreText()
  {
    scoreText.text = "Score: " + score;
  }

  IEnumerator SpawnWaves()
  {
    yield return new WaitForSeconds(startWait);
    while (!gameOver)
    {
      for (int i = 0; i < hazardCount && !gameOver; i++)
      {
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
