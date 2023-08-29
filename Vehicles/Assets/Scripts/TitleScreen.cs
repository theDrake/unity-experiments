using TMPro;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TitleScreen : MonoBehaviour {
  private TMP_InputField _nameInput;
  private TextMeshProUGUI _numEnemiesText;
  private Slider _numEnemiesSlider;
  private TMP_Dropdown _vehicleDropdown;

  private void Start() {
    _nameInput = GameObject.Find("Name Input").GetComponent<TMP_InputField>();
    _numEnemiesText =
        GameObject.Find("Enemies Number Text").GetComponent<TextMeshProUGUI>();
    _numEnemiesSlider =
        GameObject.Find("Enemies Slider").GetComponent<Slider>();
    _vehicleDropdown =
        GameObject.Find("Vehicle Dropdown").GetComponent<TMP_Dropdown>();

    _nameInput.text = CarnageManager.Instance.GetPlayerName();
    _vehicleDropdown.value =
        (int) CarnageManager.Instance.GetPlayerVehicleType();
    _numEnemiesSlider.value = CarnageManager.Instance.GetNumEnemies();
    _numEnemiesText.text = _numEnemiesSlider.value.ToString();
  }

  private void Update() {
    if (Input.GetKeyDown(KeyCode.Escape) ||
        Input.GetKeyDown(KeyCode.Backspace)) {
      Exit();
    }
  }

  public void UpdatePlayerName() {
    CarnageManager.Instance.SetPlayerName(_nameInput.text);
  }

  public void UpdatePlayerVehicle() {
    CarnageManager.Instance.SetPlayerVehicleType(_vehicleDropdown.value);
  }

  public void UpdateNumEnemies() {
    CarnageManager.Instance.SetNumEnemies((int) _numEnemiesSlider.value);
    _numEnemiesText.text = _numEnemiesSlider.value.ToString();
  }

  public void StartGame() {
    CarnageManager.Instance.StartGame();
  }

  public void Exit() {
    CarnageManager.Instance.SaveData();
#if UNITY_EDITOR
    EditorApplication.ExitPlaymode();
// #else
//     Application.Quit();
#endif
  }
}
