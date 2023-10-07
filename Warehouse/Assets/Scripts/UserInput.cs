using UnityEngine;

/// <summary>
/// Handles mouse input: left-click for selection, right-click for action.
/// </summary>
public class UserInput : MonoBehaviour {
  private const float _panSpeed = 20.0f;

  public Camera GameCamera;
  public GameObject Marker;

  private Unit _selectedUnit;
  private ResourceHolder _selectedResourceHolder;

  private void Start() {
    Marker.SetActive(false);
  }

  private void Update() {
    GameCamera.transform.position = GameCamera.transform.position +
        _panSpeed * Time.deltaTime * new Vector3(Input.GetAxis("Vertical"), 0,
                                                 -Input.GetAxis("Horizontal"));
    if (Input.GetMouseButtonDown(0)) { // left-click
      HandleSelection();
    } else if (_selectedUnit && Input.GetMouseButtonDown(1)) { // right-click
      HandleAction();
    }
  }

  public void HandleSelection() {
    if (Physics.Raycast(GameCamera.ScreenPointToRay(Input.mousePosition),
                        out RaycastHit hit)) {
      _selectedUnit = hit.collider.GetComponentInParent<Unit>();
      _selectedResourceHolder =
          hit.collider.GetComponentInParent<ResourceHolder>();
      MainCanvas.Instance.SetNewInfoContent(
          hit.collider.GetComponentInParent<MainCanvas.IInfoContent>()
      );
    }
    MarkSelectedObject();
  }

  public void HandleAction () {
    if (Physics.Raycast(GameCamera.ScreenPointToRay(Input.mousePosition),
                        out RaycastHit hit)) {
      ResourceHolder building =
          hit.collider.GetComponentInParent<ResourceHolder>();

      if (building) {
        _selectedUnit.GoTo(building);
      } else {
        _selectedUnit.GoTo(hit.point);
      }
    }
  }

  private void MarkSelectedObject() {
    if (_selectedResourceHolder &&
        Marker.transform.parent != _selectedResourceHolder.transform) {
      Marker.SetActive(true);
      Marker.transform.SetParent(_selectedResourceHolder.transform, false);
      Marker.transform.localPosition = Vector3.zero;
      if (_selectedResourceHolder.CompareTag("Storage")) {
        Marker.transform.localScale = new(3.3f, 3.3f, 3.3f);
      } else {
        Marker.transform.localScale = Vector3.one;
      }
    } else if (_selectedUnit &&
               Marker.transform.parent != _selectedUnit.transform) {
      Marker.SetActive(true);
      Marker.transform.SetParent(_selectedUnit.transform, false);
      Marker.transform.localPosition = Vector3.zero;
      Marker.transform.localScale = Vector3.one;
    } else if (!_selectedResourceHolder && !_selectedUnit &&
               Marker.activeInHierarchy) {
      Marker.SetActive(false);
      Marker.transform.SetParent(null);
    }
  }
}
