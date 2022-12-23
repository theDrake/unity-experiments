using UnityEngine;
using System.Collections;

public class LayBombs : MonoBehaviour {
  [HideInInspector]
  public bool bombLaid = false;  // Whether a bomb has currently been laid.
  public int bombCount = 0;  // How many bombs the player has.
  public AudioClip bombsAway;  // Sound for when the player lays a bomb.
  public GameObject bomb;  // Prefab of the bomb.

  private UnityEngine.UI.Image bombHUD;  // HUD of whether the player has a bomb or not.

  void Awake() {
    bombHUD = GameObject.Find("ui_bombHUD").GetComponent<UnityEngine.UI.Image>();
  }

  void Update() {
    if (Input.GetButtonDown("Fire2") && !bombLaid && bombCount > 0) {
      bombCount--;
      bombLaid = true;
      AudioSource.PlayClipAtPoint(bombsAway, transform.position);
      Instantiate(bomb, transform.position, transform.rotation);
    }
    bombHUD.enabled = bombCount > 0;
  }
}
