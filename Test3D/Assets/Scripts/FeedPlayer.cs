using UnityEngine;

public class FeedPlayer : MonoBehaviour {
  private const float _speed = 40.0f;
  private const float _xRange = 20.0f;
  private const float _zRange = 10.0f;
  private const float _y = 0;

  public GameObject foodProjectile;

  private void Update() {
    if (Input.GetKeyDown(KeyCode.Space)) {
      Instantiate(foodProjectile, transform.position,
                  foodProjectile.transform.rotation);
    }
    transform.Translate(_speed * Time.deltaTime * Input.GetAxis("Vertical") *
                        Vector3.forward);
    transform.Translate(_speed * Time.deltaTime * Input.GetAxis("Horizontal") *
                        Vector3.right);
    if (transform.position.x > _xRange) {
      transform.position = new(_xRange, _y, transform.position.z);
    } else if (transform.position.x < -_xRange) {
      transform.position = new(-_xRange, _y, transform.position.z);
    }
    if (transform.position.z > _zRange) {
      transform.position = new(transform.position.x, _y, _zRange);
    } else if (transform.position.z < -_zRange) {
      transform.position = new(transform.position.x, _y, -_zRange);
    }
  }
}
