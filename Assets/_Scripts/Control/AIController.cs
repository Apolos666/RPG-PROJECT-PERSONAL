using System;
using RPG.Combat;
using RPG.Control;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

[RequireComponent(typeof(Mover), typeof(Fighter), typeof(Health))]
public class AIController : MonoBehaviour
{
    [SerializeField] private float _chaseDistance;
    [SerializeField] private float _suspicionTime = 3f;
    [SerializeField] private PatrolPath _patrolPath;
    [SerializeField] private float _wayPointTolorance = 1f;
    [SerializeField] private float _wayPointDwellTime = 3f;

    private Mover _mover;
    private Fighter _fighter;
    private GameObject _player;
    private Health _health;
    private ActionSchedular _actionSchedular;

    private Vector3 _guardPosition;
    private float _timeSinceLastSawPlayer = Mathf.Infinity;
    private int _currentWayPointIndex = 0;
    private float _timeSinceArrivedAtWayPoint = Mathf.Infinity;

    private void Awake()
    {
        _actionSchedular = GetComponent<ActionSchedular>();
        _health = GetComponent<Health>();
        _fighter = GetComponent<Fighter>();
        _mover = GetComponent<Mover>();
        _player = GameObject.FindWithTag("Player");

        _guardPosition = transform.position;
    }

    private void Update()
    {
        if (_health.IsDead) return;
        
        if (InAttackRangeOfEnemy() && _fighter.CanAttack(_player))
        {
            AttackBehaviour();
        } else if (_timeSinceLastSawPlayer < _suspicionTime)
        {
            SuspicionBehaviour();
        }
        else
        {
            PatrolBehaviour();
        }

        UpdateTimers();
    }

    private void UpdateTimers()
    {
        _timeSinceLastSawPlayer += Time.deltaTime;
        _timeSinceArrivedAtWayPoint += Time.deltaTime;
    }

    private void SuspicionBehaviour()
    {
        _actionSchedular.CancelCurrentAction();
    }

    private void AttackBehaviour()
    {
        _timeSinceLastSawPlayer = 0f;
        _fighter.Attack(_player.gameObject);
    }

    private void PatrolBehaviour()
    {
        Vector3 nextPosition = _guardPosition;

        if (_patrolPath != null)
        {
            if (AtWayPoint())
            {
                _timeSinceArrivedAtWayPoint = 0f;
                CycleWayPoint();
            }
            
            nextPosition = GetCurrentWayPoint();
        }
        
        if (_timeSinceArrivedAtWayPoint > _wayPointDwellTime)
            _mover.StartMoveAction(nextPosition);
    }

    private void CycleWayPoint()
    {
        _currentWayPointIndex = _patrolPath.GetNextPoint(_currentWayPointIndex);
    }

    private bool AtWayPoint()
    {
        float distanceToWayPoint = Vector3.Distance(transform.position, GetCurrentWayPoint());
        return distanceToWayPoint < _wayPointTolorance;
    }

    private Vector3 GetCurrentWayPoint()
    {
        return _patrolPath.GetWayPoint(_currentWayPointIndex);
    }

    private bool InAttackRangeOfEnemy()
    {
        return Vector3.Distance(transform.position, _player.transform.position) <= _chaseDistance;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, _chaseDistance);
    }
}
