using UnityEngine;
using UnityEngine.UI;

public class WhackFoodDifficultyButton : MonoBehaviour {
  private WhackFoodManager _gameManager;
  private Button _button;
  [SerializeField] private int _difficulty;

  void Start() {
    _gameManager = FindAnyObjectByType<WhackFoodManager>();
    _button = GetComponent<Button>();
    _button.onClick.AddListener(SetDifficulty);
  }

  void SetDifficulty() {
    _gameManager.StartGame(_difficulty);
  }
}
