using UnityEngine;

[System.Serializable]
public class TankManager {
  public Color PlayerColor;
  public Transform SpawnPoint;
  [HideInInspector]
  public int PlayerNumber;
  [HideInInspector]
  public string PlayerText;
  [HideInInspector]
  public GameObject Instance;
  [HideInInspector]
  public int Wins;

  private TankMovement _movement;
  private TankShooting _shooting;
  private GameObject _canvas;

  public void Setup() {
    _movement = Instance.GetComponent<TankMovement>();
    _shooting = Instance.GetComponent<TankShooting>();
    _canvas = Instance.GetComponentInChildren<Canvas>().gameObject;
    _movement.PlayerNumber = PlayerNumber;
    _shooting.PlayerNumber = PlayerNumber;
    PlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(PlayerColor) +
        ">PLAYER " + PlayerNumber + "</color>";
    MeshRenderer[] renderers =
        Instance.GetComponentsInChildren<MeshRenderer>();
    for (int i = 0; i < renderers.Length; ++i) {
      renderers[i].material.color = PlayerColor;
    }
  }

  public void DisableControl() {
    _movement.enabled = false;
    _shooting.enabled = false;
    _canvas.SetActive(false);
  }

  public void EnableControl() {
    _movement.enabled = true;
    _shooting.enabled = true;
    _canvas.SetActive(true);
  }

  public void Reset() {
    Instance.transform.position = SpawnPoint.position;
    Instance.transform.rotation = SpawnPoint.rotation;
    Instance.SetActive(false);
    Instance.SetActive(true);
  }
}
