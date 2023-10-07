using UnityEngine;
using UnityEngine.UI;

public class InfoPopup : MonoBehaviour {
  public Text Name;
  public Text Data;
  public RectTransform ContentTransform;
  public ContentEntry EntryPrefab;

  public void ClearContent() {
    foreach (Transform child in ContentTransform) {
      Destroy(child.gameObject);
    }
  }

  public void AddToContent(int count, Sprite icon) {
    ContentEntry newEntry = Instantiate(EntryPrefab, ContentTransform);

    newEntry.CountText.text = count.ToString();
    newEntry.Icon.sprite = icon;
  }
}
