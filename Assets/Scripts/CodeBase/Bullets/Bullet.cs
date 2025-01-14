using System;
using CodeBase.Logic;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Bullets
{
    public class Bullet : MonoBehaviour
    {
        private const string EnemyTag = "Enemy";
        private const int Damage = 50;

        public event Action<GameObject> OnDeactivate;

        [SerializeField] private PopUpText _popUpText;
        
        [SerializeField] private float _speed;
        [SerializeField] private float _lifeTime;
        
        private float _timeActivated; 
        private Vector3 _direction;
        private float _timeToDestroy = 2f;

        private void Update()
        {
            if (Time.time - _timeActivated >= _lifeTime)
            {
                DeactivateBullet();
                return;
            }
            
            MoveBullet();
        }

        public void Initialize(Vector3 fireDirection)
        {
            _direction = fireDirection.normalized;
        }

        public void Active()
        {
            _timeActivated = Time.time;
        }

        private void CreatePopUp()
        {
            PopUpText popUpText = Instantiate(_popUpText);
            
            popUpText.transform.position = new Vector3(transform.position .x,2f,transform.position .z);
            popUpText.SetContextData(Damage.ToString(), Color.white);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(EnemyTag))
            {
                other.GetComponent<IHealth>().TakeDamage(Damage);
                DeactivateBullet();
            }
        }
        
        private void DeactivateBullet()
        {
            CreatePopUp();
            OnDeactivate?.Invoke(gameObject);
        }
        
        private void MoveBullet()
        {
            transform.position += _direction * (_speed * Time.deltaTime);
        }
    }
}