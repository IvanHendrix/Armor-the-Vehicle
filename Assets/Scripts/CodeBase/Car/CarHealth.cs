using System;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.GameplayControl;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Car
{
    public class CarHealth : MonoBehaviour,IHealth
    {
        public event Action HealthChanged;
        
        [SerializeField] private ActorUI _actorUI;
        
        [SerializeField] private float _current;
        [SerializeField] private float _max;

        private void Start()
        {
            LocalServices.Container.Single<IGameplayControlService>().OnGameRestart += OnRestartCarHealth;
            
            _actorUI.SetHealth(this);
        }

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

        public void TakeDamage(float damage)
        {
            Current -= damage;
            HealthChanged?.Invoke();
        }

        private void OnRestartCarHealth()
        {
            _current = _max;
        }
    }
}