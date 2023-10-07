using UnityEngine;

/// <summary>
/// ResourceHolder subclass. Repeatedly produces a given item.
/// </summary>
public class ResourceProducer : ResourceHolder {
  public ResourceItem Item;
  public float ProductionSpeed {
    get { return _productionSpeed; }
    set {
      if (value < 0) {
        Debug.LogError("Invalid production speed: " + value);
        _productionSpeed = 0;
      } else {
        _productionSpeed = value;
      }
    }
  }

  private float _productionSpeed = 0.5f;
  private float _currentProduction;

  private void Update() {
    if (_currentProduction > 1.0f) {
      int amountToAdd = Mathf.FloorToInt(_currentProduction);
      int leftOver = Store(Item.Id, amountToAdd);

      _currentProduction = _currentProduction - amountToAdd + leftOver;
    }
    if (_currentProduction < 1.0f) {
      _currentProduction += _productionSpeed * Time.deltaTime;
    }
  }

  public override string GetData() {
    if (_productionSpeed == 0) {
      return $"Producing...nothing. Useless!";
    } else if (_productionSpeed < 1) {
      return $"Producing at a rate of {_productionSpeed}/s. Nice!";
    } else {
      return $"Producing at a rate of {_productionSpeed}/s. Excellent!";
    }
  }
}
