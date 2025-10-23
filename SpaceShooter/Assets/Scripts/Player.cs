using UnityEngine;

[System.Serializable]
public class Boundary {
  public float xMin, xMax, zMin, zMax;
}

public class Player : MonoBehaviour {
  private const float speed = 8.0f;
  private const float tilt = -4.0f;
  private const float fireRate = 0.25f;

  public GameObject shot;
  public Transform shotSpawn;
  public FireZone fireZone;
  public Touchpad touchpad;
  public Boundary boundary;

  private Rigidbody _rb;
  //private Quaternion _calibrationQuaternion;
  private float _lastShotTime;

  private void Start() {
    _rb = GetComponent<Rigidbody>();
    //CalibrateAccelerometer();
  }

  private void Update() {
    // if (fireZone.CanFire && Time.time > _lastShotTime + fireRate) {
    if (Time.time > _lastShotTime + fireRate &&
        (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftControl) ||
         Input.GetKey(KeyCode.RightControl))) {
      Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
      GetComponent<AudioSource>().Play();
      _lastShotTime = Time.time;
    }
  }

  private void FixedUpdate() {
    // Keyboard controls:
    Vector3 movement = new(Input.GetAxis("Horizontal"), 0,
                           Input.GetAxis("Vertical"));

    // Accelerometer controls:
    // Vector3 acceleration = _calibrationQuaternion * Input.acceleration;
    // Vector3 movement = new(acceleration.x, 0, acceleration.y);

    // Touchpad controls:
    // Vector2 direction = touchpad.GetDirection();
    // Vector3 movement = new(direction.x, 0, direction.y);

    _rb.linearVelocity = movement * speed;
    transform.SetPositionAndRotation(new(
        Mathf.Clamp(_rb.position.x, boundary.xMin, boundary.xMax), 0,
        Mathf.Clamp(_rb.position.z, boundary.zMin, boundary.zMax)),
        Quaternion.Euler(0, 0, _rb.linearVelocity.x * tilt));
  }

  // private void CalibrateAccelerometer() {
  //   Vector3 accelerationSnapshot = Input.acceleration;
  //   Quaternion rotateQuaternion = Quaternion.FromToRotation(new(0, 0, -1.0f),
  //       accelerationSnapshot);
  //   _calibrationQuaternion = Quaternion.Inverse(rotateQuaternion);
  // }
}
