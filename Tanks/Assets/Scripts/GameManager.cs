using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
  public CameraControl CameraControl;
  public Text MessageText;
  public GameObject TankPrefab;

  private List<Tank> _tanks = new();
  private List<Transform> _spawnPoints = new();
  private Tank _roundWinner;
  private Tank _gameWinner;
  private const float _startDelay = 2.0f;
  private const float _endDelay = 2.0f;
  private const int _roundsToWin = 3;
  private WaitForSeconds _startWait;
  private WaitForSeconds _endWait;
  private int _round;
  private int _numTanks;

  private void Start() {
    foreach (GameObject o in GameObject.FindGameObjectsWithTag("SpawnPoint")) {
      _spawnPoints.Add(o.transform);
    }
    _numTanks = _spawnPoints.Count;
    _startWait = new(_startDelay);
    _endWait = new(_endDelay);
    SpawnAllTanks();
    SetCameraTargets();
    StartCoroutine(GameLoop());
  }

  private void SpawnAllTanks() {
    Shuffle(_spawnPoints);
    for (int i = 0; i < _numTanks; ++i) {
      _tanks.Add(Instantiate(TankPrefab, _spawnPoints[i].position,
                            _spawnPoints[i].rotation).GetComponent<Tank>());
      _tanks[i].PlayerNum = i + 1;
    }
  }

  private void SetCameraTargets() {
    Transform[] targets = new Transform[_numTanks];

    for (int i = 0; i < _numTanks; ++i) {
      targets[i] = _tanks[i].transform;
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
      ++_roundWinner.Wins;
    }
    _gameWinner = GetGameWinner();
    string message = EndMessage();
    MessageText.text = message;

    yield return _endWait;
  }

  private bool OneTankLeft() {
    int numTanksLeft = 0;

    foreach (Tank t in _tanks) {
      if (t.gameObject.activeSelf) {
        ++numTanksLeft;
      }
    }

    return numTanksLeft <= 1;
  }

  private Tank GetRoundWinner() {
    foreach (Tank t in _tanks) {
      if (t.gameObject.activeSelf) {
        return t;
      }
    }

    return null;
  }

  private Tank GetGameWinner() {
    foreach (Tank t in _tanks) {
      if (t.Wins == _roundsToWin) {
        return t;
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
    foreach (Tank t in _tanks) {
      message += t.PlayerText + ": " + t.Wins + " WINS\n";
    }
    if (_gameWinner != null) {
      message = _gameWinner.PlayerText + " WINS THE GAME!";
    }

    return message;
  }

  private void ResetAllTanks() {
    foreach (Tank t in _tanks) {
      t.Reset();
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

  public static void Shuffle<T>(List<T> list) {
    for (int i = list.Count - 1; i > 0; --i) {
      int j = Random.Range(0, i + 1);
      (list[i], list[j]) = (list[j], list[i]);
    }
  }
}
