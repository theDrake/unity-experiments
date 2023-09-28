using UnityEngine;

public class PlayerController : MonoBehaviour {
  private const float _speed = 20.0f;
  private const float _xRange = 20.0f;

  public GameObject projectilePrefab;

  private void Update() {
    transform.Translate(Input.GetAxis("Horizontal") * _speed * Time.deltaTime *
                        Vector3.right);
    if (transform.position.x < -_xRange) {
      transform.position = new(-_xRange, transform.position.y,
                               transform.position.z);
    } else if (transform.position.x > _xRange) {
      transform.position = new(_xRange, transform.position.y,
                               transform.position.z);
    }
    if (Input.GetKeyDown(KeyCode.Space)) {
      GameObject pooledProjectile = ObjectPooler.Instance.GetPooledObject();

      if (pooledProjectile != null) {
        pooledProjectile.SetActive(true);
        pooledProjectile.transform.position = transform.position;
      }
    }
  }
}
