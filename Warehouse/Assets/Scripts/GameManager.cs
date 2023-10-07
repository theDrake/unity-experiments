using System;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour {
  [Serializable]
  class SaveData {
    public Color SavedColor;
  }

  public static GameManager Instance { get; private set; }

  [HideInInspector] public Color TeamColor;

  private SaveData _saveData = new();
  private string _saveFile;

  private void Awake() {
    if (!Instance) {
      Instance = this;
      DontDestroyOnLoad(gameObject);
      _saveFile = Application.persistentDataPath + "/savefile.json";
      LoadGameData();
    } else {
      Destroy(gameObject);
    }
  }

  public void SaveGameData() {
    _saveData.SavedColor = TeamColor;
    File.WriteAllText(_saveFile, JsonUtility.ToJson(_saveData));
  }

  public void LoadGameData() {
    if (File.Exists(_saveFile)) {
      _saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(_saveFile));
      TeamColor = _saveData.SavedColor;
    }
  }
}
