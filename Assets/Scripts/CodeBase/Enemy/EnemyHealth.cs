using System;
using CodeBase.Logic;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class EnemyHealth : MonoBehaviour, IHealth,IResettable
    {
        public event Action HealthChanged;
        
        [SerializeField] private ActorUI _actorUI;
        
        [SerializeField] private float _current;
        [SerializeField] private float _max;

        public float Current
        {
            get => _current;
            set => _current = value;
        }

        public float Max
        {
            get => _max;
            set => _max = value;
        }

        private void Start()
        {
            _actorUI.SetHealth(this);
        }

        public void TakeDamage(float damage)
        {
            Current -= damage;
            HealthChanged?.Invoke();
        }

        public void ResetState()
        {
            _current = _max;
        }
    }
}