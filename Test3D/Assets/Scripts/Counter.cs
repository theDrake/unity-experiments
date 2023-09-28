using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour {
  private Text _countText;
  private int _count;

  private void Start() {
    _countText = GameObject.Find("Count Text").GetComponent<Text>();
  }

  private void OnTriggerExit(Collider other) {
    if (other.GetComponent<Rigidbody>().velocity.y < 0) {
      ++_count;
    } else {
      --_count;
    }
    _countText.text = "Count: " + _count;
  }
}
