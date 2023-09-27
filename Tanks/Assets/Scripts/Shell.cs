using UnityEngine;

public class Shell : MonoBehaviour {
  private const float _maxLifetime = 2.0f;
  private const float _maxDamage = 100.0f;
  private const float _explosionForce = 1000.0f;
  private const float _explosionRadius = 5.0f;

  public LayerMask ExplosionMask;
  public ParticleSystem ExplosionParticles;
  public AudioSource ExplosionAudio;

  private void Start() {
    Destroy(gameObject, _maxLifetime);
  }

  private void OnTriggerEnter(Collider other) {
    const int maxColliders = 12;
    int numColliders;
    Collider[] colliders = new Collider[maxColliders];

    numColliders = Physics.OverlapSphereNonAlloc(
        transform.position, _explosionRadius, colliders, ExplosionMask);
    for (int i = 0; i < numColliders; ++i) {
      if (colliders[i].CompareTag("DestructibleEnvironment")) {
        colliders[i].GetComponent<Destructible>().TakeDamage(CalculateDamage(
            colliders[i].transform.position));
      } else { // Tank
        colliders[i].GetComponent<Rigidbody>().AddExplosionForce(
            _explosionForce, transform.position, _explosionRadius);
        colliders[i].GetComponent<Tank>().TakeDamage(CalculateDamage(
            colliders[i].transform.position));
      }
    }
    ExplosionParticles.transform.parent = null;
    ExplosionParticles.Play();
    ExplosionAudio.Play();
    Destroy(ExplosionParticles.gameObject, ExplosionParticles.main.duration);
    Destroy(gameObject);
  }

  private float CalculateDamage(Vector3 targetPosition) {
    float explosionDistance = (targetPosition - transform.position).magnitude;
    float relativeDistance = (_explosionRadius - explosionDistance) /
        _explosionRadius;

    return Mathf.Max(0f, relativeDistance * _maxDamage);
  }
}
