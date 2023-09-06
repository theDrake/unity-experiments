using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
  public CameraControl CameraControl;
  public Text MessageText;
  public GameObject TankPrefab;
  public const int MaxTanks = 8;

  private MainMenu _menu;
  private readonly static List<Tank> _tanks = new();
  private readonly List<Destructible> _destructibles = new();
  private readonly List<Transform> _spawnPoints = new();
  private readonly List<Color> _teamColors = new();
  private const float _startDelay = 1.0f;
  private const float _endDelay = 3.0f;
  private WaitForSeconds _startWait;
  private WaitForSeconds _endWait;
  private readonly int[] _wins = new int[MaxTanks];
  private const int _roundsToWin = 3;
  private int _roundWinner;
  private int _gameWinner;
  private int _round;
  private static int _numTeams = 2;
  private static int _numTanksPerTeam = 4;
  private static int _numTanks;

  private void Start() {
    foreach (GameObject o in GameObject.FindGameObjectsWithTag("SpawnPoint")) {
      _spawnPoints.Add(o.transform);
    }
    foreach (GameObject o in GameObject.FindGameObjectsWithTag(
             "DestructibleEnvironment")) {
      _destructibles.Add(o.GetComponent<Destructible>());
    }
    _menu = FindAnyObjectByType<MainMenu>();
    _menu.TeamsSlider.value = _numTeams;
    _menu.TanksPerTeamSlider.value = _numTanksPerTeam;
    _startWait = new(_startDelay);
    _endWait = new(_endDelay);
    MessageText.gameObject.SetActive(false);
    _teamColors.Add(Color.black);
    _teamColors.Add(Color.white);
    _teamColors.Add(Color.grey);
    _teamColors.Add(Color.red);
    _teamColors.Add(Color.green);
    _teamColors.Add(Color.blue);
    _teamColors.Add(Color.cyan);
    _teamColors.Add(Color.yellow);
    _teamColors.Add(Color.magenta);
  }

  public void StartGame() {
    _numTeams = (int) _menu.TeamsSlider.value;
    _numTanksPerTeam = (int) _menu.TanksPerTeamSlider.value;
    _numTanks = _numTeams * _numTanksPerTeam;
    _menu.gameObject.SetActive(false);
    MessageText.gameObject.SetActive(true);
    for (int i = 0; i < _wins.Length; ++i) {
      _wins[i] = 0;
    }
    _roundWinner = _gameWinner = 0;
    ShuffleList(_teamColors);
    SpawnAllTanks();
    SetCameraTargets();
    StartCoroutine(GameLoop());
  }

  public static Tank FindTargetForTank(int tankNum) {
    List<Tank> sublist = new();

    for (int i = 0; i < _numTanks; ++i) {
      if (i != tankNum - 1 && _tanks[i].gameObject.activeSelf &&
          _tanks[i].TeamNum != _tanks[tankNum - 1].TeamNum) {
        sublist.Add(_tanks[i]);
      }
    }
    if (sublist.Count > 0) {
      return sublist[Random.Range(0, sublist.Count)];
    }

    return null;
  }

  public static void ShuffleList<T>(List<T> list) {
    int i, j;

    for (i = list.Count - 1; i > 0; --i) {
      j = Random.Range(0, i + 1);
      (list[i], list[j]) = (list[j], list[i]);
    }
  }

  private void SpawnAllTanks() {
    _tanks.Clear();
    ShuffleList(_spawnPoints);
    for (int i = 0; i < _numTanks; ++i) {
      _tanks.Add(Instantiate(TankPrefab, _spawnPoints[i].position,
                             _spawnPoints[i].rotation).GetComponent<Tank>());
      _tanks[i].TankNum = i + 1;
      _tanks[i].TeamNum = (i / _numTanksPerTeam) + 1;
      _tanks[i].TankColor = _teamColors[i / _numTanksPerTeam];
    }
  }

  private void SetCameraTargets() {
    Transform[] targets = new Transform[_tanks.Count];

    foreach (Tank t in _tanks) {
      targets[t.TankNum - 1] = t.transform;
    }
    CameraControl.Targets = targets;
  }

  private IEnumerator GameLoop() {
    yield return StartCoroutine(RoundStarting());
    yield return StartCoroutine(RoundPlaying());
    yield return StartCoroutine(RoundEnding());

    if (_gameWinner > 0) {
      SceneManager.LoadScene(0);
    } else {
      StartCoroutine(GameLoop());
    }
  }

  private IEnumerator RoundStarting() {
    ResetAllTanks();
    ResetAllDestructibles();
    DisableTankControl();
    CameraControl.SetStartPositionAndSize();
    MessageText.text = "ROUND " + ++_round;

    yield return _startWait;
  }

  private IEnumerator RoundPlaying() {
    EnableTankControl();
    MessageText.text = string.Empty;

    while (!RoundComplete()) {
      yield return null;
    }
  }

  private IEnumerator RoundEnding() {
    DisableTankControl();
    if (_roundWinner > 0 && ++_wins[_roundWinner - 1] >= _roundsToWin) {
      _gameWinner = _roundWinner;
    }
    MessageText.text = GetRoundCompleteMessage();

    yield return _endWait;
  }

  private bool RoundComplete() {
    Tank firstTank = null;

    foreach (Tank t in _tanks) {
      if (t.gameObject.activeSelf) {
        if (firstTank == null) {
          firstTank = t;
        } else if (t.TeamNum != firstTank.TeamNum) {
          return false;
        }
      }
    }
    if (firstTank) {
      _roundWinner = firstTank.TeamNum;
    } else {
      _roundWinner = 0;
    }

    return true;
  }

  private string GetRoundCompleteMessage() {
    string message = "DRAW!";

    if (_roundWinner > 0) {
      message = GetTeamText(_roundWinner) + " WINS THE ROUND!";
    }
    message += "\n\n\n\n";
    for (int i = 0; i < _numTeams; ++i) {
      message += GetTeamText(i + 1) + ": " + _wins[i] + " WINS\n";
    }
    if (_gameWinner > 0) {
      message = GetTeamText(_gameWinner) + " WINS THE GAME!";
    }

    return message;
  }

  private string GetTeamText(int teamNum) {
    string text = "<color=#" + ColorUtility.ToHtmlStringRGB(
        _teamColors[teamNum - 1]);

    if (_numTanksPerTeam == 1) {
      text += ">TANK ";
    } else {
      text += ">TEAM ";
    }
    text += teamNum + "</color>";

    return text;
  }

  private void ResetAllTanks() {
    foreach (Tank t in _tanks) {
      t.Reset();
    }
  }

  private void ResetAllDestructibles() {
    foreach (Destructible d in _destructibles) {
      d.Reset();
    }
  }

  private void EnableTankControl() {
    foreach (Tank t in _tanks) {
      t.EnableControl();
    }
  }

  private void DisableTankControl() {
    foreach (Tank t in _tanks) {
      t.DisableControl();
    }
  }
}
