using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.GameplayControl;
using CodeBase.Services.World;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private int _initialEnemyCount = 5;
        [SerializeField] private float _spawnDistanceCar = 50f;
        [SerializeField] private float _enemySpawnDistance = 50f;
        [SerializeField] private int _enemiesPerSpawn = 5;

        [SerializeField] private EnemyPool _enemyPool;

        private GameObject _target;
        private float _lastSpawnPosition = 0f;

        private bool _start;
        
        private void Start()
        {
            LocalServices.Container.Single<IGameplayControlService>().OnGameStart += OnInitEnemies;
            LocalServices.Container.Single<IGameplayControlService>().OnGameRestart += OnRestartEnemies;
            LocalServices.Container.Single<IGameplayControlService>().OnGamePause += OnPause;
            
            _enemyPool.OnChasedTarget += OnChasedTarget;
            _enemyPool.OnEnemySlayedEvent += OnCollectedCoins;
            
            SpawnInitialEnemies();
        }

        private void Update()
        {
            if (!_start)
            {
                return;
            }
            
            if (_target.transform.position.z - _lastSpawnPosition >= _spawnDistanceCar)
            {
                SpawnEnemies();
                _lastSpawnPosition = _target.transform.position.z;
            }
        }

        public void SetTarget(GameObject target)
        {
            _target = target;
            
            _enemyPool.Construct(_target.transform,40);
        }

        private void OnPause()
        {
            _start = false;
        }

        private void OnCollectedCoins()
        {
            LocalServices.Container.Single<IWorldStateService>().Collect(15);
        }

        private void OnRestartEnemies()
        {
            _start = false;
            _lastSpawnPosition = 0;
            _enemyPool.Reload();
        }

        private void OnInitEnemies()
        {
            _start = true;
            SpawnInitialEnemies();
        }

        private void OnChasedTarget()
        {
            _target.GetComponent<IHealth>().TakeDamage(10);
        }

        private void SpawnInitialEnemies()
        {
            for (int i = 0; i < _initialEnemyCount; i++)
            {
                SpawnEnemy();
            }
        }

        private void SpawnEnemies()
        {
            for (int i = 0; i < _enemiesPerSpawn; i++)
            {
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            GameObject enemy = _enemyPool.GetEnemy();
            
            float spawnZPosition = _target.transform.position.z + _enemySpawnDistance + Random.Range(15f, 30f); 
            enemy.transform.position = new Vector3(Random.Range(-5f, 5f), 0f, spawnZPosition);
        }

        public void Reload()
        {
            _enemyPool.Reload();
            _lastSpawnPosition = 0;
        }
    }
}