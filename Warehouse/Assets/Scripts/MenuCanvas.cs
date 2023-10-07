using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

[DefaultExecutionOrder(1000)]
public class MenuCanvas : MonoBehaviour {
  private ColorPicker _colorPicker;

  private void Start() {
    _colorPicker = GetComponentInChildren<ColorPicker>();
    _colorPicker.Init();
    _colorPicker.OnColorChanged += NewColorSelected;
    _colorPicker.SelectColor(GameManager.Instance.TeamColor);
  }

  public void NewColorSelected(Color color) {
    GameManager.Instance.TeamColor = color;
  }

  public void SaveColor() {
    GameManager.Instance.SaveGameData();
  }

  public void LoadColor() {
    GameManager.Instance.LoadGameData();
    _colorPicker.SelectColor(GameManager.Instance.TeamColor);
  }

  public void StartGame() {
    GameManager.Instance.SaveGameData();
    SceneManager.LoadScene(1);
  }

  public void Exit() {
    GameManager.Instance.SaveGameData();
#if UNITY_EDITOR
    EditorApplication.ExitPlaymode();
#else
    Application.Quit();
#endif
  }
}
