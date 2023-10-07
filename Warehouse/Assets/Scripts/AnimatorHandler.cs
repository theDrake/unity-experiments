using UnityEngine;
using UnityEngine.AI;

public class AnimatorHandler : MonoBehaviour {
  private Animator _animator;
  private NavMeshAgent _agent;

  private void Start() {
    _animator = GetComponentInChildren<Animator>();
    _agent = GetComponentInParent<NavMeshAgent>();
  }

  private void Update() {
    if (_agent && _animator) {
      _animator.SetFloat("Speed", _agent.velocity.magnitude / _agent.speed);
    }
  }
}
