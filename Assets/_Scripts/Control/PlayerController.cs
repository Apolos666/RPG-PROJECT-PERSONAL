using System;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    [RequireComponent(typeof(Mover), typeof(Fighter), typeof(Health))]
    public class PlayerController : MonoBehaviour
    {
        private Mover _mover;
        private Camera _camera;
        private Fighter _fighter;
        private Health _health;

        private void Awake()
        {
            _health = GetComponent<Health>();
            _fighter = GetComponent<Fighter>();
            _mover = GetComponent<Mover>();
            _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

        private void Update()
        {
            if (_health.IsDead) return;
            
            if (InteractWithCombat()) return;
            if (InteractWithMove()) return;
        }

        private bool InteractWithMove()
        {
            bool hasHit = Physics.Raycast(GetMouseRayFromCamera(), out RaycastHit hit);
            
            if (hasHit && Input.GetMouseButton(0))
                _mover.StartMoveAction(hit.point);

            return hasHit;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRayFromCamera());

            foreach (var hit in hits)
            {
                CombatTarget combatTarget = hit.transform.GetComponent<CombatTarget>();

                if (combatTarget == null) continue;
                
                if (!_fighter.CanAttack(combatTarget.gameObject)) continue;
                
                if (Input.GetMouseButton(0))
                    _fighter.Attack(combatTarget.gameObject);

                return true;
            }

            return false;
        }

        private Ray GetMouseRayFromCamera()
        {
            return _camera.ScreenPointToRay(Input.mousePosition);
        }
    }
}


