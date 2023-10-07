using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unit subclass. Transfers items from a ResourceProducer to storage.
/// </summary>
public class TransporterUnit : Unit {
  private const float _defaultTransporterSpeed = _defaultSpeed * 2.0f;

  private ResourceHolder _transportTarget;
  private ResourceHolder.InventoryEntry _transporting = new();
  [SerializeField] private int _maxTransportAmount = 5;

  protected override void Start() {
    base.Start();
    _speed = _defaultTransporterSpeed;
  }

  public override string GetName() {
    return "Transporter";
  }

  public override string GetData() {
    if (_maxTransportAmount <= 0) {
      return $"Can transport...nothing. Pointless!";
    } else if (_maxTransportAmount == 1) {
      return $"Can transport one item. That's all, folks!";
    } else if (_maxTransportAmount >= 10) {
      return $"Can transport up to {_maxTransportAmount} items. Wowee!";
    } else {
      return $"Can transport up to {_maxTransportAmount} items. Not bad!";
    }
  }

  public override void GetContent(ref List<ResourceHolder.InventoryEntry>
                                  content) {
    if (_transporting.Count > 0) {
      content.Add(_transporting);
    }
  }

  public override void GoTo(Vector3 position) {
    base.GoTo(position);
    _transportTarget = null;
  }

  protected override void ResourceHolderInRange() {
    if (_target.CompareTag("Storage")) {
      if (_transporting.Count > 0) {
        _target.Store(_transporting.ResourceIdStr, _transporting.Count);
      }
      GoTo(_transportTarget);
      _transporting.Count = 0;
      _transporting.ResourceIdStr = "";
    } else {
      if (_target.Inventory.Count > 0) {
        _transporting.ResourceIdStr = _target.Inventory[0].ResourceIdStr;
        _transporting.Count = _target.Remove(_transporting.ResourceIdStr,
                                             _maxTransportAmount);
        _transportTarget = _target;
        GoTo(GameObject.FindWithTag("Storage").GetComponent<ResourceHolder>());
      }
    }
  }
}
