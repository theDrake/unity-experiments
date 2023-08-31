using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour {
  public static StartMenu Instance { get; private set; }
  public TMP_InputField Username;

  private void Awake() {
    if (Instance == null) {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    } else {
      Destroy(gameObject);
    }
  }

  public void LoadMainScene() {
    SceneManager.LoadScene(1);
  }
}
