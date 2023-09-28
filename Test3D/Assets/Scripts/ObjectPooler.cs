using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {
  public static ObjectPooler Instance { get; private set; }

  public List<GameObject> pooledObjects;
  public GameObject objectToPool;
  public int amountToPool;

  private void Awake() {
    if (Instance == null) {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    } else {
      Destroy(gameObject);
    }
  }

  private void Start() {
    pooledObjects = new List<GameObject>();
    for (int i = 0; i < amountToPool; ++i) {
      GameObject obj = (GameObject) Instantiate(objectToPool);
      obj.SetActive(false);
      pooledObjects.Add(obj);
      obj.transform.SetParent(this.transform);
    }
  }

  public GameObject GetPooledObject() {
    for (int i = 0; i < pooledObjects.Count; ++i) {
      if (!pooledObjects[i].activeInHierarchy) {
        return pooledObjects[i];
      }
    }

    return null;
  }
}
