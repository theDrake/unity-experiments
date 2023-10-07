using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for objects that can store or produce items.
/// </summary>
public class ResourceHolder : MonoBehaviour, MainCanvas.IInfoContent {
  [System.Serializable]
  public class InventoryEntry {
    public string ResourceIdStr;
    public int Count;
  }

  public List<InventoryEntry> Inventory => _inventory;

  protected List<InventoryEntry> _inventory = new();
  protected int _numItemsStored = 0;
  [Tooltip("-1 is infinite")]
  [SerializeField] protected int _maxInventory = -1;

  private void Start() {
    if (_maxInventory < 0) {
      _maxInventory = System.Int32.MaxValue;
    }
  }

  public virtual string GetName() {
    return gameObject.name;
  }

  public virtual string GetData() {
    if (_maxInventory == System.Int32.MaxValue) {
      return $"Can hold an infinite number of items. Incredible!";
    } else if (_maxInventory == 0) {
      return $"Can hold zero items. Useless!";
    } else if (_maxInventory == 1) {
      return $"Can hold one item. Wow!";
    } else {
      return $"Can hold a total of {_maxInventory} items. Impressed?";
    }
  }

  public void GetContent(ref List<InventoryEntry> content) {
    content.AddRange(_inventory);
  }

  /// <returns>Amount NOT stored</returns>
  public int Store(string resourceId, int amount) {
    if (_numItemsStored >= _maxInventory) {
      return amount;
    }
    int index = _inventory.FindIndex(item => item.ResourceIdStr == resourceId);
    int amountStored = Mathf.Min(_maxInventory - _numItemsStored, amount);

    if (index == -1) {
      _inventory.Add(new InventoryEntry() {
        Count = amountStored,
        ResourceIdStr = resourceId
      });
    } else {
      _inventory[index].Count += amountStored;
    }
    _numItemsStored += amountStored;

    return amount - amountStored;
  }

  /// <returns>Amount removed.</returns>
  public int Remove(string resourceId, int requestAmount) {
    int index = _inventory.FindIndex(item => item.ResourceIdStr == resourceId);

    if (index != -1) {
      int amount = Mathf.Min(requestAmount, _inventory[index].Count);

      _inventory[index].Count -= amount;
      if (_inventory[index].Count == 0) {
        _inventory.RemoveAt(index);
      }
      _numItemsStored -= amount;

      return amount;
    }

    return 0;
  }
}
