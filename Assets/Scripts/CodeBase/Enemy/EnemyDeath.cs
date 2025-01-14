using System;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyHealth))]
    public class EnemyDeath : MonoBehaviour
    {
        private const int Coin = 15;
        public event Action<GameObject> OnHappened;

        [SerializeField] private PopUpText _popUpText;
        [SerializeField] private GameObject _deathFX;

        [SerializeField] private EnemyHealth _health;
        private float _timeToDestroy = 2f;

        private void Start()
        {
            _health.HealthChanged += OnHealthChanged;
        }

        public void PlayDeath()
        {
            CreateDeathFX();
        }

        private void OnHealthChanged()
        {
            if (_health.Current <= 0)
            {
                CreatePopUp();
                Die();
            }
        }

        private void CreatePopUp()
        {
            PopUpText popUpText = Instantiate(_popUpText);
            popUpText.transform.position = transform.position + new Vector3(0, 2f, 0);
            popUpText.SetContextData(Coin.ToString(), Color.yellow);
            Destroy(popUpText, _timeToDestroy);
        }

        private void CreateDeathFX()
        {
            GameObject deathFX = Instantiate(_deathFX);
            deathFX.transform.position = transform.position;
            Destroy(deathFX, _timeToDestroy);
        }

        private void Die()
        {
            CreateDeathFX();
            OnHappened?.Invoke(gameObject);
        }
    }
}