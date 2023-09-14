using TMPro;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TitleScreen : MonoBehaviour {
  private TMP_Dropdown _vehicleDropdown;
  private TextMeshProUGUI _numEnemiesText;
  private TextMeshProUGUI _numObstaclesText;
  private Slider _numEnemiesSlider;
  private Slider _numObstaclesSlider;

  private void Start() {
    _vehicleDropdown =
        GameObject.Find("Vehicle Dropdown").GetComponent<TMP_Dropdown>();
    _numEnemiesText = GameObject.Find(
        "Enemies Number Text").GetComponent<TextMeshProUGUI>();
    _numEnemiesSlider = GameObject.Find(
        "Enemies Slider").GetComponent<Slider>();
    _numObstaclesText = GameObject.Find(
        "Obstacles Number Text").GetComponent<TextMeshProUGUI>();
    _numObstaclesSlider = GameObject.Find(
        "Obstacles Slider").GetComponent<Slider>();

    _vehicleDropdown.value = (int) GameManager.Instance.GetPlayerVehicleType();
    _numEnemiesSlider.value = GameManager.Instance.GetNumEnemies();
    _numEnemiesText.text = _numEnemiesSlider.value.ToString();
    _numObstaclesSlider.value = GameManager.Instance.GetNumObstacles();
    _numObstaclesText.text = _numObstaclesSlider.value.ToString();
  }

  private void Update() {
    if (Input.GetKeyDown(KeyCode.Escape) ||
        Input.GetKeyDown(KeyCode.Backspace)) {
      Exit();
    }
  }

  public void UpdatePlayerVehicle() {
    GameManager.Instance.SetPlayerVehicleType(_vehicleDropdown.value);
  }

  public void UpdateNumEnemies() {
    GameManager.Instance.SetNumEnemies((int) _numEnemiesSlider.value);
    _numEnemiesText.text = _numEnemiesSlider.value.ToString();
  }

  public void UpdateNumObstacles() {
    GameManager.Instance.SetNumObstacles((int) _numObstaclesSlider.value);
    _numObstaclesText.text = _numObstaclesSlider.value.ToString();
  }

  public void StartGame() {
    GameManager.Instance.StartGame();
  }

  public void Exit() {
    GameManager.Instance.SaveData();
#if UNITY_EDITOR
    EditorApplication.ExitPlaymode();
// #else
//     Application.Quit();
#endif
  }
}
