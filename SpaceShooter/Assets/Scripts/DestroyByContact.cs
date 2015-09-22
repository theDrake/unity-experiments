using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour
{
  public GameObject explosion, playerExplosion;

  private Transform thisTransform, otherTransform;

  void Start()
  {
    thisTransform = GetComponent<Transform>();
  }

  void OnTriggerEnter(Collider other)
  {
    if (other.tag == "Boundary")
      return;
    Destroy(gameObject);
    Destroy(other.gameObject);
    Instantiate(explosion, thisTransform.position, thisTransform.rotation);
    if (other.tag == "Player")
    {
      otherTransform = other.GetComponent<Transform>();
      Instantiate(playerExplosion,
                  otherTransform.position,
                  otherTransform.rotation);
    }
  }
}
