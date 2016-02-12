using UnityEngine;
using System.Collections;

public class BackgroundPropSpawner : MonoBehaviour {
  public Rigidbody2D backgroundProp;
  public float leftSpawnPosX;
  public float rightSpawnPosX;
  public float minSpawnPosY;
  public float maxSpawnPosY;
  public float minTimeBetweenSpawns;
  public float maxTimeBetweenSpawns;
  public float minSpeed;
  public float maxSpeed;

  void Start() {
    Random.seed = System.DateTime.Today.Millisecond;
    StartCoroutine("Spawn");
  }

  IEnumerator Spawn() {
    float waitTime = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
    yield return new WaitForSeconds(waitTime);
    bool facingLeft = Random.Range(0, 2) == 0;
    float posX = facingLeft ? rightSpawnPosX : leftSpawnPosX;
    float posY = Random.Range(minSpawnPosY, maxSpawnPosY);
    Vector3 spawnPos = new Vector3(posX, posY, transform.position.z);
    Rigidbody2D propInstance = Instantiate(backgroundProp, spawnPos,
                                           Quaternion.identity) as Rigidbody2D;

    // Sprites for props all face left. Therefore, if prop should face right:
    if (!facingLeft) {
      Vector3 scale = propInstance.transform.localScale;
      scale.x *= -1;
      propInstance.transform.localScale = scale;
    }

    float speed = Random.Range(minSpeed, maxSpeed);
    speed *= facingLeft ? -1f : 1f;
    propInstance.velocity = new Vector2(speed, 0);
    StartCoroutine(Spawn());
    while (propInstance != null) {
      if (facingLeft) {
        if (propInstance.transform.position.x < leftSpawnPosX - 0.5f) {
          Destroy(propInstance.gameObject);
        }
      } else {
        if (propInstance.transform.position.x > rightSpawnPosX + 0.5f) {
          Destroy(propInstance.gameObject);
        }
      }

      // Return to this point after the next update.
      yield return null;
    }
  }
}
