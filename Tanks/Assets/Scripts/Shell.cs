using UnityEngine;

public class Shell : MonoBehaviour {
  public LayerMask TankMask;
  public ParticleSystem ExplosionParticles;
  public AudioSource ExplosionAudio;

  private const float _maxLifetime = 2.0f;
  private const float _maxDamage = 100.0f;
  private const float _explosionForce = 1000.0f;
  private const float _explosionRadius = 5.0f;

  private void Start() {
    Destroy(gameObject, _maxLifetime);
  }

  private void OnTriggerEnter(Collider other) {
    foreach (Collider c in Physics.OverlapSphere(transform.position,
                                                 _explosionRadius, TankMask)) {
      Rigidbody target = c.GetComponent<Rigidbody>();
      if (!target) {
        continue;
      }
      target.AddExplosionForce(_explosionForce, transform.position,
                               _explosionRadius);
      Tank tank = target.GetComponent<Tank>();
      if (tank) {
        tank.TakeDamage(CalculateDamage(target.position));
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
