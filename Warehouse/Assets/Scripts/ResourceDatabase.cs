using System.Collections.Generic;
using UnityEngine;

public class ResourceDatabase : ScriptableObject {
  public List<ResourceItem> ResourceTypes = new();

  private Dictionary<string, ResourceItem> _database;

  public void Init() {
    _database = new();
    foreach (ResourceItem item in ResourceTypes) {
      _database.Add(item.Id, item);
    }
  }

  public ResourceItem GetItem(string uniqueId) {
    _database.TryGetValue(uniqueId, out ResourceItem type);

    return type;
  }
}
