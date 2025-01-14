using CodeBase.Services;
using CodeBase.Services.GameplayControl;
using UnityEngine;

namespace CodeBase.Ground
{
    public class RoadLoop : MonoBehaviour
    {
        [SerializeField] private GameObject _roadPrefab;
        [SerializeField] private int _poolSize;
        [SerializeField] private float _spawnDistance;
        [SerializeField] private float _modeGroundDistance;
        
        private Transform _target;
        
        private GameObject[] _roadSegments;
        private int _currentSegmentIndex = 0;
        private int _step = 0;

        private bool _isMoving;
        
        private void Start()
        {
            LocalServices.Container.Single<IGameplayControlService>().OnGameStart += OnStartMovement;
            LocalServices.Container.Single<IGameplayControlService>().OnGameRestart += OnRestart;
            LocalServices.Container.Single<IGameplayControlService>().OnGamePause += OnPause;
            
            _roadSegments = new GameObject[_poolSize];
            _step = _poolSize;
            
            for (int i = 0; i < _poolSize; i++)
            {
                _roadSegments[i] = Instantiate(_roadPrefab, new Vector3(0, 0, i * _spawnDistance), Quaternion.identity);
            }
        }

        private void Update()
        {
            if (!_isMoving)
            {
                return;
            }
            
            MovingGrounds();
        }

        private void OnPause()
        {
            _isMoving = false;
        }

        private void OnRestart()
        {
            _isMoving = false;
            _step = _poolSize;
            _currentSegmentIndex = 0;

            for (int i = 0; i < _roadSegments.Length; i++)
            {
                _roadSegments[i].transform.position = new Vector3(0, 0, _spawnDistance * i);
            }
        }

        private void OnStartMovement()
        {
            _isMoving = true;
        }

        public void SetTarget(GameObject target)
        {
            _target = target.transform;
        }

        private void MovingGrounds()
        {
            if (_target.position.z > _roadSegments[_currentSegmentIndex].transform.position.z + _modeGroundDistance)
            {
                _roadSegments[_currentSegmentIndex].transform.position = new Vector3(0, 0, _step * _spawnDistance);

                _currentSegmentIndex = (_currentSegmentIndex + 1) % _poolSize;
                _step++;
            }
        }
    }
}