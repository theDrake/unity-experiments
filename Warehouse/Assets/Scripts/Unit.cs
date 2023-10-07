using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Base class for Units. Movement handled via NavMeshAgent.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public abstract class Unit : MonoBehaviour, MainCanvas.IInfoContent {
  protected const float _defaultSpeed = 3.0f;

  protected NavMeshAgent _agent;
  protected ResourceHolder _target;
  protected float _speed = _defaultSpeed;

  protected virtual void Awake() {
    _agent = GetComponent<NavMeshAgent>();
    _agent.speed = _speed;
    _agent.acceleration = 999;
    _agent.angularSpeed = 999;
  }

  protected virtual void Start() {
    if (GameManager.Instance) {
      SetColor(GameManager.Instance.TeamColor);
    }
  }

  protected virtual void Update() {
    if (_target && Vector3.Distance(_target.transform.position,
                                    transform.position) < 2.0f) {
      _agent.isStopped = true;
      ResourceHolderInRange();
    }
  }

  public virtual string GetName() {
    return "Unit";
  }

  public virtual string GetData() {
    return "";
  }

  public virtual void GetContent(ref List<ResourceHolder.InventoryEntry>
                                 content) {}

  public virtual void GoTo(Vector3 position) {
    _target = null;
    _agent.SetDestination(position);
    _agent.isStopped = false;
  }

  public virtual void GoTo(ResourceHolder target) {
    _target = target;
    if (_target) {
      _agent.SetDestination(_target.transform.position);
      _agent.isStopped = false;
    }
  }

  protected abstract void ResourceHolderInRange();

  protected virtual void SetColor(Color c) {
    ColorHandler colorHandler = GetComponentInChildren<ColorHandler>();

    if (colorHandler) {
      colorHandler.SetColor(c);
    }
  }
}
