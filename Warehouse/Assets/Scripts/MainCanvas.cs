using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCanvas : MonoBehaviour {
  public interface IInfoContent {
    string GetName();
    string GetData();
    void GetContent(ref List<ResourceHolder.InventoryEntry> content);
  }

  public static MainCanvas Instance { get; private set; }

  public ResourceDatabase ResourceDb;

  private InfoPopup _infoPopup;
  private IInfoContent _currentContent;
  private List<ResourceHolder.InventoryEntry> _contentBuffer = new();

  private void Awake() {
    if (!Instance) {
      Instance = this;
      _infoPopup = GetComponentInChildren<InfoPopup>();
      _infoPopup.gameObject.SetActive(false);
      ResourceDb.Init();
    } else {
      Destroy(gameObject);
    }
  }

  private void Update() {
    if (_currentContent == null) {
      return;
    }
    _infoPopup.Data.text = _currentContent.GetData();
    _infoPopup.ClearContent();
    _contentBuffer.Clear();
    _currentContent.GetContent(ref _contentBuffer);
    foreach (ResourceHolder.InventoryEntry entry in _contentBuffer) {
      Sprite icon = null;

      if (ResourceDb && ResourceDb.GetItem(entry.ResourceIdStr)) {
        icon = ResourceDb.GetItem(entry.ResourceIdStr).Icon;
      }
      if (icon) {
        _infoPopup.AddToContent(entry.Count, icon);
      }
    }
  }

  public void SetNewInfoContent(IInfoContent content) {
    if (content == null) {
      _infoPopup.gameObject.SetActive(false);
    } else {
      _infoPopup.gameObject.SetActive(true);
      _currentContent = content;
      _infoPopup.Name.text = content.GetName();
    }
  }

  public void ReturnToStartMenu() {
    SceneManager.LoadScene(0);
  }

  private void OnDestroy() {
    Instance = null;
  }
}
