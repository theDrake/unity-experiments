using TMPro;
using UnityEngine;

public class CarnageCanvas : MonoBehaviour {
  private TextMeshProUGUI _speedText;
  private Vehicle _player;

  private void Start() {
    _speedText = GameObject.Find("Speed Text").GetComponent<TextMeshProUGUI>();
    _player = FindAnyObjectByType<Player>().GetComponent<Vehicle>();
  }

  private void Update() {
    float speed = _player ? _player.GetSpeed() : 0;

    _speedText.text = "Speed: " + GetMph(speed) + " mph / " + GetKph(speed) +
        " kph";
  }

  private float GetMph(float n) {
    return Mathf.Round(n * 2.237f);
  }

  private float GetKph(float n) {
    return Mathf.Round(n * 3.6f);
  }
}
