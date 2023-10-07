using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour {
  private readonly Color[] _availableColors = {
    Color.red,
    Color.green,
    Color.blue,
    Color.cyan,
    Color.magenta,
    Color.yellow,
  };
  private readonly List<Button> _colorButtons = new();

  public Color SelectedColor { get; private set; }
  public System.Action<Color> OnColorChanged;
  public Button ColorButtonPrefab;

  public void Init() {
    foreach (Color c in _availableColors) {
      Button button = Instantiate(ColorButtonPrefab, transform);

      button.GetComponent<Image>().color = c;
      button.onClick.AddListener(() => {
        SelectedColor = c;
        foreach (Button b in _colorButtons) {
          b.interactable = true;
        }
        button.interactable = false;
        OnColorChanged.Invoke(SelectedColor);
      });
      _colorButtons.Add(button);
    }
  }

  public void SelectColor(Color color) {
    _colorButtons[0].onClick.Invoke(); // default color
    for (int i = 0; i < _availableColors.Length; ++i) {
      if (_availableColors[i] == color) {
        _colorButtons[i].onClick.Invoke();
      }
    }
  }
}
