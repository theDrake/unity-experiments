using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE
public class Airplane : Vehicle {
  protected float _forwardSpeed = 30;
  protected float _rotationSpeed = 150;

  // POLYMORPHISM
  public override void Move(float verticalInput, float horizontalInput) {
    transform.Translate(Vector3.forward * Time.deltaTime * _forwardSpeed);
    transform.Rotate(_rotationSpeed * verticalInput * Time.deltaTime *
                     Vector3.right);
    transform.Rotate(_rotationSpeed * horizontalInput * Time.deltaTime *
                     Vector3.forward);
  }
}
