using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {
  public int score = 0;  // The player's score.

  private PlayerControl playerControl;  // Reference to player control script.
  private int previousScore = 0;  // Score from previous frame.

  void Awake() {
    playerControl =
      GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
  }

  void Update() {
    GetComponent<GUIText>().text = "Score: " + score;
    if (previousScore != score)
      playerControl.StartCoroutine(playerControl.Taunt());
    previousScore = score;
  }
}
