using System;
using CodeBase.Services;
using CodeBase.Services.GameplayControl;
using CodeBase.Services.World;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Car
{
    public class CarMovement : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 10f;
        [SerializeField] private float _driftAmount = 2f;
        [SerializeField] private float _driftSpeed = 2f; 
        [SerializeField] private float _driftInterval = 5f;
        
        private float _smoothDamping = 0.1f; 
        private float _nextDriftTime = 0f;
        private float _currentDrift = 0f;
        private float _targetDrift = 0f;

        private bool _isMoving;
        
        private void Start()
        {
            LocalServices.Container.Single<IGameplayControlService>().OnGameStart += OnStartMovement;
            LocalServices.Container.Single<IGameplayControlService>().OnGameRestart += OnRestart;
            LocalServices.Container.Single<IGameplayControlService>().OnGamePause += OnPause;
        }

        private void Update()
        {
            if (!_isMoving)
            {
                return;
            }
            
            Movement();
        }

        private void Movement()
        {
            float moveForward = _moveSpeed * Time.deltaTime;

            if (Time.time >= _nextDriftTime)
            {
                _targetDrift = Random.Range(-_driftAmount, _driftAmount);
                _nextDriftTime = Time.time + _driftInterval;
            }

            _currentDrift = Mathf.Lerp(_currentDrift, _targetDrift, _driftSpeed * Time.deltaTime);
            transform.position = new Vector3(_currentDrift, transform.position.y, transform.position.z + moveForward);
            
            LocalServices.Container.Single<IWorldStateService>().UpdateFinishedDistance(transform.position.z);
        }

        private void OnPause()
        {
            _isMoving = false;
        }

        private void OnRestart()
        {
            _isMoving = false;
            transform.position = Vector3.zero;
        }

        private void OnStartMovement()
        {
            _isMoving = true;
        }
    }
}