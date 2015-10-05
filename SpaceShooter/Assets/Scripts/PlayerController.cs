using UnityEngine;

[System.Serializable]
public class Boundary
{
  public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
  public float speed, tilt, fireRate;
  public Boundary boundary;
  public GameObject shot;
  public Transform shotSpawn;
  public Touchpad touchpad;

  private float lastShotTime;
  private Rigidbody rb;
  //private Quaternion calibrationQuaternion;

  void Start()
  {
    rb = GetComponent<Rigidbody>();
    //CalibrateAccelerometer();
  }

  void Update()
  {
    if (Input.GetButton("Fire1") && Time.time > lastShotTime + fireRate)
    {
      Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
      GetComponent<AudioSource>().Play();
      lastShotTime = Time.time;
    }
  }

  void FixedUpdate()
  {
    // Keyboard controls:
    //float moveHorizontal = Input.GetAxis("Horizontal");
    //float moveVertical = Input.GetAxis("Vertical");
    //Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

    // Accelerometer controls:
    //Vector3 acceleration = calibrationQuaternion * Input.acceleration;
    //Vector3 movement = new Vector3(acceleration.x, 0.0f, acceleration.y);

    Vector2 direction = touchpad.GetDirection();
    Vector3 movement = new Vector3(direction.x, 0.0f, direction.y);

    rb.velocity = movement * speed;
    rb.position = new Vector3
      (
        Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
        0.0f,
        Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
      );
    rb.rotation = Quaternion.Euler
      (
        0.0f,
        0.0f,
        rb.velocity.x * tilt
      );
  }

  //void CalibrateAccelerometer()
  //{
  //  Vector3 accelerationSnapshot = Input.acceleration;
  //  Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, -1.0f),
  //                                                          accelerationSnapshot);
  //  calibrationQuaternion = Quaternion.Inverse(rotateQuaternion);
  //}
}
