using System;
using CodeBase.Logic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Enemy
{
    public class EnemyMovement : MonoBehaviour, IResettable
    {
        public event Action<GameObject> OnChasedTarget;

        [SerializeField] private float _moveSpeed = 3f;
        [SerializeField] private float _detectionRange = 10f;
        [SerializeField] private float _minDistanceToTarget;
        [SerializeField] private float _offSetDistance;

        [SerializeField] private EnemyDeath _enemyDeath;
        [SerializeField] private EnemyAnimator _enemyAnimator;
        [SerializeField] private GameObject _model;
        
        private Transform _target;
        private bool _isChasing = false;
        private bool _isFinishChase = false;

        private Vector3 _randomDirection;
        private float _waitTime;
        private float _moveTime; 
        private float _moveTimer;
        private float _waitTimer;
        private bool _isStandingStill;
    

        private void Start()
        {
            SetRandomMovement();
        }

        private void Update()
        {
            if (_isFinishChase)
            {
                return;
            }

            CheckCarBehindPosition();

            CheckDistance();

            if (_isChasing)
            {
                ChasingTarget();
                return;
            }

            MoveRandomly();
        }

        public void SetChaseTarget(Transform target)
        {
            _target = target;
        }

        public void ResetState()
        {
            _isChasing = false;
            _isFinishChase = false;
            _isStandingStill = false;
            SetRandomMovement();
        }

        private void CheckCarBehindPosition()
        {
            if (transform.position.z < _target.position.z + _offSetDistance)
            {
                OnChasedTarget?.Invoke(gameObject);
                _isFinishChase = true;
            }
        }

        private void CheckDistance()
        {
            float distance = Vector3.Distance(transform.position, _target.position);

            if (distance < _minDistanceToTarget)
            {
                OnChasedTarget?.Invoke(gameObject);
                _isFinishChase = true;
                return;
            }

            if (distance < _detectionRange)
            {
                _enemyDeath.PlayDeath();
                _isChasing = true;
            }
        }

        private void MoveRandomly()
        {
            if (_moveTimer > 0)
            {
                if (_randomDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(_randomDirection);
                    _model.transform.rotation = Quaternion.Slerp(_model.transform.rotation, targetRotation, Time.deltaTime * _moveSpeed);
                }
                
                transform.position += _randomDirection * _moveSpeed * Time.deltaTime;
                _moveTimer -= Time.deltaTime;
            }
            else
            {
                if (_waitTimer > 0)
                {
                    _waitTimer -= Time.deltaTime;
                }
                else
                {
                    SetRandomMovement();
                }
            }
        }

        private void SetRandomMovement()
        {
            _isStandingStill = Random.Range(0, 2) == 0 ? false : true;

            if (!_isStandingStill)
            {
                _enemyAnimator.SetMovingState(true);
                _enemyAnimator.SetSpeedAnimation(0);
                
                _randomDirection =
                    new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
                _moveTime = Random.Range(2f, 5f);
                _waitTime = Random.Range(1f, 3f);

                _moveTimer = _moveTime;
                _waitTimer = _waitTime;
            }
            else
            {
                _enemyAnimator.SetMovingState(false);

                _moveTimer = 0;
                _waitTimer = Random.Range(2f, 5f);
            }
        }

        private void ChasingTarget()
        {
            _enemyAnimator.SetMovingState(true);
            _enemyAnimator.SetSpeedAnimation(1);

            Vector3 direction = (_target.position - transform.position).normalized;
            transform.position += direction * _moveSpeed * Time.deltaTime;
            
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _model.transform.rotation = Quaternion.Slerp(_model.transform.rotation, targetRotation, Time.deltaTime * _moveSpeed);
        }
    }
}