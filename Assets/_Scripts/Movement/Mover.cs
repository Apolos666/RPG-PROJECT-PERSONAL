using System;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(ActionSchedular))]
    public class Mover : MonoBehaviour, IAction
    {
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private ActionSchedular _actionSchedular;
        private Health _health;
        
        private static readonly int ForwardSpeed = Animator.StringToHash("ForwardSpeed");

        private void Awake()
        {
            _health = GetComponent<Health>();
            _actionSchedular = GetComponent<ActionSchedular>();
            _animator = GetComponent<Animator>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            _navMeshAgent.enabled = !_health.IsDead;
            
            LocomotionAnimation();
        }

        private void LocomotionAnimation()
        {
            Vector3 globalVelocity = _navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(globalVelocity);
            float speed = localVelocity.z;
            float forwardSpeed = Mathf.InverseLerp(0, _navMeshAgent.speed, speed);
            _animator.SetFloat(ForwardSpeed, forwardSpeed);
        }

        public void StartMoveAction(Vector3 destination)
        {
            _actionSchedular.StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.destination = destination;
        }

        public void Cancel()
        {
            _navMeshAgent.isStopped = true;
        }
    }
}


