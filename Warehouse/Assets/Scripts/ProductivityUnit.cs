using UnityEngine;

/// <summary>
/// Unit subclass that can increase ResourceProducer efficiency.
/// </summary>
public class ProductivityUnit : Unit {
  private ResourceProducer _productionTarget;
  [SerializeField] private float _productivityMultiplier = 2.0f;

  public override string GetName() {
    return "Worker";
  }

  public override string GetData() {
    string output =
        $"Increases nearby production rate by {_productivityMultiplier}×.";

    if (_productivityMultiplier <= 0) {
      output += " Pathetic!";
    } else if (_productivityMultiplier >= 3.0f) {
      output += " Amazing!";
    } else {
      output += " Not too shabby!";
    }

    return output;
  }

  public override void GoTo(Vector3 position) {
    ResetProductivity();
    base.GoTo(position);
  }

  public override void GoTo(ResourceHolder target) {
    ResetProductivity();
    base.GoTo(target);
  }

  protected override void ResourceHolderInRange() {
    if (!_productionTarget) {
      ResourceProducer producer = _target as ResourceProducer;

      if (producer) {
        _productionTarget = producer;
        _productionTarget.ProductionSpeed *= _productivityMultiplier;
      }
    }
  }

  private void ResetProductivity() {
    if (_productionTarget) {
      _productionTarget.ProductionSpeed /= _productivityMultiplier;
      _productionTarget = null;
    }
  }
}
