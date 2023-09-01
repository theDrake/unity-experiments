using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
  public CameraControl CameraControl;
  public Text MessageText;
  public GameObject TankPrefab;
  public TankManager[] Tanks;

  private const float _startDelay = 2.0f;
  private const float _endDelay = 2.0f;
  private const int _roundsToWin = 3;
  private int _round;
  private WaitForSeconds _startWait;
  private WaitForSeconds _endWait;
  private TankManager _roundWinner;
  private TankManager _gameWinner;

  private void Start() {
    _startWait = new(_startDelay);
    _endWait = new(_endDelay);
    SpawnAllTanks();
    SetCameraTargets();
    StartCoroutine(GameLoop());
  }

  private void SpawnAllTanks() {
    for (int i = 0; i < Tanks.Length; ++i) {
      Tanks[i].Instance = Instantiate(TankPrefab, Tanks[i].SpawnPoint.position,
          Tanks[i].SpawnPoint.rotation) as GameObject;
      Tanks[i].PlayerNumber = i + 1;
      Tanks[i].Setup();
    }
  }

  private void SetCameraTargets() {
    Transform[] targets = new Transform[Tanks.Length];

    for (int i = 0; i < targets.Length; ++i) {
      targets[i] = Tanks[i].Instance.transform;
    }
    CameraControl.Targets = targets;
  }

  private IEnumerator GameLoop() {
    yield return StartCoroutine(RoundStarting());
    yield return StartCoroutine(RoundPlaying());
    yield return StartCoroutine(RoundEnding());

    if (_gameWinner != null) {
      SceneManager.LoadScene(0);
    } else {
      StartCoroutine(GameLoop());
    }
  }

  private IEnumerator RoundStarting() {
    ResetAllTanks();
    DisableTankControl();
    CameraControl.SetStartPositionAndSize();
    MessageText.text = "ROUND " + ++_round;

    yield return _startWait;
  }

  private IEnumerator RoundPlaying() {
    EnableTankControl();
    MessageText.text = string.Empty;

    while (!OneTankLeft()) {
      yield return null;
    }
  }

  private IEnumerator RoundEnding() {
    DisableTankControl();
    _roundWinner = null;
    _roundWinner = GetRoundWinner();
    if (_roundWinner != null) {
      _roundWinner.Wins++;
    }
    _gameWinner = GetGameWinner();
    string message = EndMessage();
    MessageText.text = message;

    yield return _endWait;
  }

  private bool OneTankLeft() {
    int numTanksLeft = 0;

    for (int i = 0; i < Tanks.Length; ++i) {
      if (Tanks[i].Instance.activeSelf) {
        ++numTanksLeft;
      }
    }

    return numTanksLeft <= 1;
  }

  private TankManager GetRoundWinner() {
    for (int i = 0; i < Tanks.Length; ++i) {
      if (Tanks[i].Instance.activeSelf) {
        return Tanks[i];
      }
    }

    return null;
  }

  private TankManager GetGameWinner() {
    for (int i = 0; i < Tanks.Length; ++i) {
      if (Tanks[i].Wins == _roundsToWin) {
        return Tanks[i];
      }
    }

    return null;
  }

  private string EndMessage() {
    string message = "DRAW!";

    if (_roundWinner != null) {
      message = _roundWinner.PlayerText + " WINS THE ROUND!";
    }
    message += "\n\n\n\n";
    for (int i = 0; i < Tanks.Length; ++i) {
      message += Tanks[i].PlayerText + ": " + Tanks[i].Wins + " WINS\n";
    }
    if (_gameWinner != null) {
      message = _gameWinner.PlayerText + " WINS THE GAME!";
    }

    return message;
  }

  private void ResetAllTanks() {
    for (int i = 0; i < Tanks.Length; ++i) {
      Tanks[i].Reset();
    }
  }

  private void EnableTankControl() {
    for (int i = 0; i < Tanks.Length; ++i) {
      Tanks[i].EnableControl();
    }
  }

  private void DisableTankControl() {
    for (int i = 0; i < Tanks.Length; ++i) {
      Tanks[i].DisableControl();
    }
  }
}
