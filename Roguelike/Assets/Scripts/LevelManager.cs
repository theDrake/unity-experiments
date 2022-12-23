using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour {
  [Serializable]
  public class Count {
    public int min, max;

    public Count(int minimum, int maximum) {
      min = minimum;
      max = maximum;
    }
  }

  public int columns = 8;
  public int rows = 8;
  public Count wallCount = new Count(5, 9); // min/max per level
  public Count foodCount = new Count(1, 5); // min/max per level
  public GameObject exit;
  public GameObject[] floorTiles;
  public GameObject[] wallTiles;
  public GameObject[] outerWallTiles;
  public GameObject[] foodTiles;
  public GameObject[] enemyTiles;

  private Transform objectHolder;
  private List<Vector3> gridPositions = new List<Vector3>();

  void InitializeGrid() {
    gridPositions.Clear();
    for (int x = 1; x < columns - 1; ++x) {
      for (int y = 1; y < rows - 1; ++y) {
        gridPositions.Add(new Vector3(x, y, 0f));
      }
    }
  }

  void SetOuterWallAndFloorTiles() {
    objectHolder = new GameObject("Level").transform;
    for (int x = -1; x < columns + 1; ++x) {
      for (int y = -1; y < rows + 1; ++y) {
        GameObject tile = floorTiles[Random.Range(0, floorTiles.Length)];
        if (x == -1 || x == columns || y == -1 || y == rows) {
          tile = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
        }
        GameObject instance = Instantiate(tile, new Vector3(x, y, 0f),
                                          Quaternion.identity) as GameObject;
        instance.transform.SetParent(objectHolder);
      }
    }
  }

  void SetRandomTilesAtRandomPositions(GameObject[] tileArray, int numTiles) {
    for (int i = 0; i < numTiles; ++i) {
      Vector3 position = RandomPosition();
      GameObject tile = tileArray[Random.Range(0, tileArray.Length)];
      Instantiate(tile, position, Quaternion.identity);
    }
  }

  public void InitializeLevel(int level) {
    SetOuterWallAndFloorTiles();
    InitializeGrid();
    SetRandomTilesAtRandomPositions(wallTiles,
                                    Random.Range(wallCount.min,
                                                 wallCount.max + 1));
    SetRandomTilesAtRandomPositions(foodTiles,
                                    Random.Range(foodCount.min,
                                                 foodCount.max + 1));
    int numEnemies = (int) Mathf.Log(level, 2.0f);
    SetRandomTilesAtRandomPositions(enemyTiles, numEnemies);
    Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f),
                Quaternion.identity);
  }

  Vector3 RandomPosition() {
    int randomIndex = Random.Range(0, gridPositions.Count);
    Vector3 randomPosition = gridPositions[randomIndex];
    gridPositions.RemoveAt(randomIndex);

    return randomPosition;
  }
}
