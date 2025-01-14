using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Bullets
{
    public class BulletsPool : MonoBehaviour
    {
        [SerializeField] private GameObject _bulletPrefab;

        private Transform _firePoint;
        
        private Queue<GameObject> _bulletQueue;

        public void Construct(Transform firePoint, int poolSize)
        {
            _firePoint = firePoint;
            CreatePool(poolSize);
        }

        public Bullet GetBullet()
        {
            if (_bulletQueue.Count > 0)
            {
                Bullet bullet = _bulletQueue.Dequeue().GetComponent<Bullet>();
                bullet.Initialize(_firePoint.forward);
                bullet.Active();
                
                bullet.gameObject.SetActive(true);
                return bullet;
            }
            
            GameObject item = Instantiate(_bulletPrefab);
            Bullet newBullet = item.GetComponent<Bullet>();
            newBullet.GetComponent<Bullet>().OnDeactivate += OnBulletDeactivate;
            newBullet.Initialize(_firePoint.forward);
            newBullet.Active();
            
            _bulletQueue.Enqueue(newBullet.gameObject);

            return newBullet;
        }

        private void CreatePool(int poolSize)
        {
            _bulletQueue = new Queue<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                GameObject bullet = Instantiate(_bulletPrefab);
                bullet.GetComponent<Bullet>().OnDeactivate += OnBulletDeactivate;
                bullet.SetActive(false);
                _bulletQueue.Enqueue(bullet);
            }
        }

        private void OnBulletDeactivate(GameObject obj)
        {
            ReturnBullet(obj);
        }

        private void ReturnBullet(GameObject bullet)
        {
            bullet.gameObject.SetActive(false);
            
            _bulletQueue.Enqueue(bullet);
        }

        public void Reload()
        {
            foreach (var enemy in _bulletQueue)
            {
                enemy.SetActive(false);
            }
        }
    }
}