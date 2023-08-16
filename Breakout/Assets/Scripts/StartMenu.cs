using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartMenu : MonoBehaviour {
  public static StartMenu Instance;
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
