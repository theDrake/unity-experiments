using UnityEngine;
using TMPro;

public class Player : MonoBehaviour {
  [SerializeField] private Vehicle _vehicle;
  [SerializeField] private TextMeshProUGUI _speedText;
  // [SerializeField] private float _forwardSpeed = 80;
  // [SerializeField] private float _turnSpeed = 60;
  // private float _forwardForce = 10000000.0f;
  private int _speed;
  private bool _useMetricSystem;

  void Start() {
    _useMetricSystem = false;
    if (!_vehicle) {
      _vehicle = GetComponent<Vehicle>();
    }
  }

  void FixedUpdate() {
    float verticalInput = Input.GetAxis("Vertical");
    float horizontalInput = Input.GetAxis("Horizontal");

    if (_vehicle) {
      Debug.Log("Moving: " + verticalInput.ToString() + ", " +
                horizontalInput.ToString());
      _vehicle.Move(verticalInput, horizontalInput);
    }
    // transform.Translate(Vector3.forward * Time.deltaTime * _forwardSpeed *
    //   verticalInput);
    // _rigidBody.AddRelativeForce(Vector3.forward * _forwardForce *
    //                             verticalInput * Time.deltaTime);
    // transform.Rotate(Vector3.up * Time.deltaTime * _turnSpeed *
    //                  horizontalInput);
  }

  void Update() {
    if (_useMetricSystem) {
      _speed = Mathf.RoundToInt(_vehicle.GetSpeed() * 3.6f); // kph
      _speedText.text = "Speed: " + _speed + " KPH";
    } else {
      _speed = Mathf.RoundToInt(_vehicle.GetSpeed() * 2.237f); // mph
      _speedText.text = "Speed: " + _speed + " MPH";
    }
  }
}
