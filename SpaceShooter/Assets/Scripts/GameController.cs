using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
  public GameObject hazard;
  public Vector3 spawnValues;
  public GUIText scoreText;
  public int hazardCount;
  public float startWait, spawnWait, waveWait;

  private int score;

  void Start()
  {
    score = 0;
    UpdateScoreText();
    StartCoroutine(SpawnWaves());
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
    while (true)
    {
      for (int i = 0; i < hazardCount; i++)
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
