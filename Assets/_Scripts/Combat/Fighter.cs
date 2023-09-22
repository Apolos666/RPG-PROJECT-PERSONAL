using System;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Mover), typeof(Animator), typeof(ActionSchedular))]
    public class Fighter : MonoBehaviour, IAction
    {
        private Health _target;
        private Mover _mover;
        private Animator _animator;
        private ActionSchedular _actionSchedular;

        [SerializeField] private float _weaponRange = 2f;
        [SerializeField] private float _timeBetweenAttacks = 1f;
        [SerializeField] private float _weaponDamage = 5f;

        private float _timeSinceLastAttack = Mathf.Infinity;
        
        private static readonly int AttackAnimation = Animator.StringToHash("Attack");
        private static readonly int CancelAttack = Animator.StringToHash("CancelAttack");

        private void Awake()
        {
            _actionSchedular = GetComponent<ActionSchedular>();
            _animator = GetComponent<Animator>();
            _mover = GetComponent<Mover>();
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            
            if (_target == null) return;
            if (_target.IsDead) return;

            if (!GetIsInRange())
            {
                _mover.MoveTo(_target.transform.position);
            }
            else
            {
                _mover.Cancel();
                AttackBehaviour();
            }
        }

        private void Hit()
        {
            if (_target == null) return;
            _target.TakeDamage(_weaponDamage);
        }

        private void AttackBehaviour()
        {
            transform.LookAt(_target.transform);
            
            if (_timeSinceLastAttack >= _timeBetweenAttacks)
            {
                TriggerAttack();
                _timeSinceLastAttack = 0f;
            }
        }

        private void TriggerAttack()
        {
            _animator.ResetTrigger(CancelAttack);
            _animator.SetTrigger(AttackAnimation);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, _target.transform.position) <= _weaponRange;
        }

        public void Attack(GameObject combatTarget)
        {
            _actionSchedular.StartAction(this);
            _target = combatTarget.GetComponent<Health>();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead;
        }

        public void Cancel()
        {
            StopAttackAnimation();
            _target = null;
        }

        private void StopAttackAnimation()
        {
            _animator.ResetTrigger(AttackAnimation);
            _animator.SetTrigger(CancelAttack);
        }
    }
}

