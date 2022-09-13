using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class UnitAI : MonoBehaviour
{
    public float checkRate = 1.0f;
    public float nearbyEnemyAttackRange;

    public LayerMask unitLayerMask;

    private PlayerAI _playerAI;
    private Unit _unit;

    private void Awake()
    {
        _playerAI = FindObjectOfType<PlayerAI>();
        _unit = GetComponent<Unit>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(Check), 0.0f, checkRate);
    }

    private void Check()
    {
        if (_unit.state != UnitState.Attack && _unit.state != UnitState.MoveToEnemy)
        {
            Unit potentialEnemy = CheckForNearbyEnemies();

            if (potentialEnemy != null)
            {
                _unit.AttackUnit(potentialEnemy);
            }
        }

        if (_unit.state == UnitState.Idle)
        {
            FindNewResource();
        }
        else if (_unit.state == UnitState.MoveToResource && _unit.curResourceSource == null)
        {
            FindNewResource();
        }
    }

    private void FindNewResource()
    {
        ResourceSource resourceToGet = _playerAI.GetClosestResource(transform.position);

        if (resourceToGet != null)
        {
            _unit.GatherResource(resourceToGet, UnitMover.GetUnitDestinationAroundResource(resourceToGet.transform.position));
        }
        else
        {
            PursueEnemy();
        }
    }

    private Unit CheckForNearbyEnemies()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, nearbyEnemyAttackRange, Vector3.up, unitLayerMask);

        GameObject closest = null;
        float closestDist = 0.0f;

        for (int x = 0; x < hits.Length; x++)
        {
            if (hits[x].collider.gameObject == gameObject)
            {
                continue;
            }

            if (_unit.player.IsMyUnit(hits[x].collider.GetComponent<Unit>()))
            {
                continue;
            }

            if (!closest || Vector3.Distance(transform.position, hits[x].transform.position) < closestDist)
            {
                closest = hits[x].collider.gameObject;
                closestDist = Vector3.Distance(transform.position, hits[x].transform.position);
            }
        }

        if (closest != null)
        {
            return closest.GetComponent<Unit>();
        }
        else
        {
            return null;
        }
    }

    private void PursueEnemy()
    {
        Player enemyPlayer = GameManager.Instance.GetRandomEnemyPlayer(_unit.player);

        if (enemyPlayer.units.Count > 0)
        {
            _unit.AttackUnit(enemyPlayer.units[Random.Range(0, enemyPlayer.units.Count)]);
        }
    }
}
