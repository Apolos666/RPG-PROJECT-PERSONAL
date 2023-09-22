using UnityEngine;

namespace RPG.Core
{
    [RequireComponent(typeof(Animator), typeof(ActionSchedular))]
    public class Health : MonoBehaviour
    {
        [SerializeField] private float _healthPoints = 100f;

        private bool _isDead;
        public bool IsDead => _isDead;

        private Animator _animator;
        private ActionSchedular _actionSchedular;
        
        private static readonly int DeathAnimation = Animator.StringToHash("Die");

        private void Awake()
        {
            _actionSchedular = GetComponent<ActionSchedular>();
            _animator = GetComponent<Animator>();
        }

        public void TakeDamage(float damage)
        {
            if (_isDead) return;
            
            _healthPoints = Mathf.Max(_healthPoints - damage, 0);

            if (_healthPoints == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            _isDead = true;
            _animator.SetTrigger(DeathAnimation);
            _actionSchedular.CancelCurrentAction();
        }
    }
}

