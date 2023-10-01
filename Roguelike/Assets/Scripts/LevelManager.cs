using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
  [System.Serializable]
  private class Count {
    public int Min, Max;

    public Count(int minimum, int maximum) {
      Min = minimum;
      Max = maximum;
    }
  }

  private readonly Count _wallCount = new(5, 9); // min/max per level
  private readonly Count _foodCount = new(1, 5); // min/max per level
  private readonly List<Vector3> _grid = new();
  private const int _columns = 8;
  private const int _rows = 8;

  [SerializeField] private GameObject[] _floorTiles;
  [SerializeField] private GameObject[] _wallTiles;
  [SerializeField] private GameObject[] _outerWallTiles;
  [SerializeField] private GameObject[] _foodTiles;
  [SerializeField] private GameObject[] _enemyTiles;
  [SerializeField] private GameObject _exitTile;
  private Transform _levelTransform;

  public void InitializeLevel(int level) {
    int numEnemies = (int) Mathf.Log(level, 2.0f);

    DestroyAllWithTag("Enemy", "Food", "Soda", "Floor", "Exit", "Wall",
                      "OuterWall");
    SetOuterWallAndFloorTiles();
    InitializeGrid();
    SetAdditionalTiles(_wallTiles, Random.Range(_wallCount.Min,
                                                _wallCount.Max + 1));
    SetAdditionalTiles(_foodTiles, Random.Range(_foodCount.Min,
                                                _foodCount.Max + 1));
    SetAdditionalTiles(_enemyTiles, numEnemies);
    Instantiate(_exitTile, new(level % 2 == 0 ? 0 : _columns - 1,
                               level % 2 == 0 ? 0 : _rows - 1),
                Quaternion.identity);
  }

  private void SetOuterWallAndFloorTiles() {
    _levelTransform = new GameObject("Level").transform;

    for (int x = -1; x < _columns + 1; ++x) {
      for (int y = -1; y < _rows + 1; ++y) {
        GameObject tile = _floorTiles[Random.Range(0, _floorTiles.Length)];

        if (x == -1 || x == _columns || y == -1 || y == _rows) {
          tile = _outerWallTiles[Random.Range(0, _outerWallTiles.Length)];
        }
        GameObject Instance = Instantiate(tile, new(x, y), Quaternion.identity)
            as GameObject;
        Instance.transform.SetParent(_levelTransform);
      }
    }
  }

  private void SetAdditionalTiles(GameObject[] tiles, int numTiles) {
    for (int i = 0; i < numTiles; ++i) {
      Instantiate(tiles[Random.Range(0, tiles.Length)], GetRandomGridPoint(),
                  Quaternion.identity);
    }
  }

  private void InitializeGrid() {
    _grid.Clear();
    for (int x = 1; x < _columns - 1; ++x) {
      for (int y = 1; y < _rows - 1; ++y) {
        _grid.Add(new(x, y));
      }
    }
  }

  private Vector3 GetRandomGridPoint() {
    int randomIndex = Random.Range(0, _grid.Count);
    Vector3 position = _grid[randomIndex];

    _grid.RemoveAt(randomIndex);

    return position;
  }

  private void DestroyAllWithTag(params string[] tags) {
    foreach(string tag in tags) {
      foreach(GameObject o in GameObject.FindGameObjectsWithTag(tag)) {
        Destroy(o);
      }
    }
  }
}
