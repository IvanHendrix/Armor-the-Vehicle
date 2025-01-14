using CodeBase.Bullets;
using CodeBase.Services;
using CodeBase.Services.GameplayControl;
using UnityEngine;

namespace CodeBase.Turret
{
    public class TurretFire : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private float _fireRate = 1f;

        [SerializeField] private BulletsPool _bulletsPool;

        private float _nextFireTime = 0f;

        private bool _isShooting;
        
        private void Start()
        {
            LocalServices.Container.Single<IGameplayControlService>().OnGameRestart += OnRestart;
            LocalServices.Container.Single<IGameplayControlService>().OnGameStart += OnInit;
            LocalServices.Container.Single<IGameplayControlService>().OnGamePause += OnPause;

            _bulletsPool.Construct(_firePoint, 50);
        }

        private void Update()
        {
            if (!_isShooting)
            {
                return;
            }
            
            Shooting();
            ShowLaserPointer();
        }

        private void OnPause()
        {
            _isShooting = false;
        }

        private void OnInit()
        {
            _isShooting = true;
        }

        private void OnRestart()
        {
            _isShooting = false;
            
            _nextFireTime = 0;
            _bulletsPool.Reload();
        }

        private void Shooting()
        {
            if (Time.time >= _nextFireTime)
            {
                Fire();
                _nextFireTime = Time.time + 1f / _fireRate;
            }
        }

        private void Fire()
        {
            Bullet bullet = _bulletsPool.GetBullet();
            bullet.transform.position = _firePoint.position;
        }

        private void ShowLaserPointer()
        {
            _lineRenderer.SetPosition(0, _firePoint.position);
            _lineRenderer.SetPosition(1, _firePoint.position + _firePoint.forward * 30f);
        }
    }
}