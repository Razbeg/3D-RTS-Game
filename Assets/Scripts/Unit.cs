using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public enum UnitState
{
    Idle,
    Move,
    MoveToResource,
    Gather,
    MoveToEnemy,
    Attack
}

public class Unit : MonoBehaviour
{
    [Header("Stats")] 
    public UnitState state;

    public int curHp;
    public int maxHp;

    public int minAttackDamage;
    public int maxAttackDamage;

    public float attackRate;
    private float _lastAttackTime;

    public float attackDistance;

    public float pathUpdateRate = 1.0f;
    private float _lastPathUpdateTime;

    public int gatherAmount;
    public float gatherRate;
    public ResourceSource curResourceSource;

    private float _lastGatherTime;
    private Unit _curEnemyTarget;
    
    [Header("Components")]
    public GameObject selectionVisual;
    public UnitHealthBar healthBar;

    private NavMeshAgent _navAgent;

    public Player player;
    
    [Serializable]
    public class StateChangeEvent : UnityEvent<UnitState> { }
    public StateChangeEvent onStateChange;

    private void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        
        SetState(UnitState.Idle);
    }

    private void Update()
    {
        switch (state)
        {
            case UnitState.Move:
                MoveUpdate();
                break;
            case UnitState.MoveToResource:
                MoveToResourceUpdate();
                break;
            case UnitState.Gather:
                GatherUpdate();
                break;
            case UnitState.MoveToEnemy:
                MoveToEnemyUpdate();
                break;
            case UnitState.Attack:
                AttackUpdate();
                break;
        }
    }

    private void MoveUpdate()
    {
        if (Vector3.Distance(transform.position, _navAgent.destination) == 0.0f)
        {
            SetState(UnitState.Idle);
        }
    }

    private void MoveToResourceUpdate()
    {
        if (curResourceSource == null)
        {
            SetState(UnitState.Idle);
            return;
        }
        
        if (Vector3.Distance(transform.position, _navAgent.destination) == 0.0f)
        {
            SetState(UnitState.Gather);
        }
    }

    private void GatherUpdate()
    {
        if (curResourceSource == null)
        {
            SetState(UnitState.Idle);
            return;
        }
        
        LookAt(curResourceSource.transform.position);

        if (Time.time - _lastGatherTime > gatherRate)
        {
            _lastGatherTime = Time.time;
            curResourceSource.GatherResource(gatherAmount, player);
        }
    }

    private void MoveToEnemyUpdate()
    {
        if (_curEnemyTarget == null)
        {
            SetState(UnitState.Idle);
            return;
        }

        if (Time.time - _lastPathUpdateTime > pathUpdateRate)
        {
            _lastPathUpdateTime = Time.time;
            _navAgent.isStopped = false;
            _navAgent.SetDestination(_curEnemyTarget.transform.position);
        }

        if (Vector3.Distance(transform.position, _curEnemyTarget.transform.position) <= attackDistance)
        {
            SetState(UnitState.Attack);
        }
    }

    private void AttackUpdate()
    {
        if (_curEnemyTarget == null)
        {
            SetState(UnitState.Idle);
            return;
        }

        if (!_navAgent.isStopped)
        {
            _navAgent.isStopped = true;
        }

        if (Time.time - _lastAttackTime > attackRate)
        {
            _lastAttackTime = Time.time;
            _curEnemyTarget.TakeDamage(Random.Range(minAttackDamage, maxAttackDamage + 1));
        }
        
        LookAt(_curEnemyTarget.transform.position);

        if (Vector3.Distance(transform.position, _curEnemyTarget.transform.position) > attackDistance)
        {
            SetState(UnitState.MoveToEnemy);
        }
    }

    private void SetState(UnitState toState)
    {
        state = toState;

        if (onStateChange != null)
        {
            onStateChange.Invoke(state);
        }

        if (toState == UnitState.Idle)
        {
            _navAgent.isStopped = true;
            _navAgent.ResetPath();
        }
    }

    public void TakeDamage(int damage)
    {
        curHp -= damage;

        if (curHp <= 0)
        {
            Die();
        }
        
        healthBar.UpdateHealthBar(curHp, maxHp);
    }

    private void Die()
    {
        player.units.Remove(this);
        GameManager.Instance.UnitDeathCheck();
        Destroy(gameObject);
    }

    public void MoveToPosition(Vector3 pos)
    {
        SetState(UnitState.Move);

        if (_navAgent)
        {
            _navAgent.isStopped = false;
            _navAgent.SetDestination(pos);
        }
    }

    public void GatherResource(ResourceSource resource, Vector3 pos)
    {
        curResourceSource = resource;
        SetState(UnitState.MoveToResource);
        
        _navAgent.isStopped = false;
        _navAgent.SetDestination(pos);
    }

    public void AttackUnit(Unit target)
    {
        _curEnemyTarget = target;
        SetState(UnitState.MoveToEnemy);
    }

    public void ToggleSelectionVisual(bool selected)
    {
        if (selectionVisual != null)
        {
            selectionVisual.SetActive(selected);
        }
    }

    private void LookAt(Vector3 pos)
    {
        Vector3 dir = (pos - transform.position).normalized;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, angle, 0);
    }
}
