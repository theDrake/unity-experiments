using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
  public Text TeamsText;
  public Text TanksPerTeamText;
  public Slider TeamsSlider;
  public Slider TanksPerTeamSlider;

  private const int _maxTanks = 8;

  public void UpdateTeams() {
    TeamsText.text = "Teams: " + TeamsSlider.value;
    TanksPerTeamSlider.maxValue = _maxTanks / (int) TeamsSlider.value;
  }

  public void UpdateTanksPerTeam() {
    TanksPerTeamText.text = "Tanks per team: " + TanksPerTeamSlider.value;
  }
}
