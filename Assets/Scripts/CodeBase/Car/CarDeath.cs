using CodeBase.Services;
using CodeBase.Services.GameplayControl;
using UnityEngine;

namespace CodeBase.Car
{
    public class CarDeath : MonoBehaviour
    {
        [SerializeField] private CarHealth _health;
        
        private void Start()
        {
            _health.HealthChanged += OnHealthChanged;
        }

        private void OnDestroy()
        {
            _health.HealthChanged -= OnHealthChanged;
        }
        
        private void OnHealthChanged()
        {
            if (_health.Current <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            LocalServices.Container.Single<IGameplayControlService>().SetGameLose();
        }
    }
}