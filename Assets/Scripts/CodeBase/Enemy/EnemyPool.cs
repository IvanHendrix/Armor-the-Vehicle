using System;
using System.Collections.Generic;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class EnemyPool : MonoBehaviour
    {
        public event Action OnChasedTarget;
        public event Action OnEnemySlayedEvent;

        [SerializeField] private GameObject _enemyPrefab;
        
        private Queue<GameObject> _enemyQueue;
        private Transform _target;

        public void Construct(Transform target, int poolSize)
        {
            _target = target;

            CreatePool(poolSize);
        }

        public GameObject GetEnemy()
        {
            if (_enemyQueue.Count > 0)
            {
                GameObject enemy = _enemyQueue.Dequeue();
                enemy.SetActive(true);
                return enemy;
            }

            GameObject newEnemy = Instantiate(_enemyPrefab, transform);
            newEnemy.GetComponent<EnemyMovement>().SetChaseTarget(_target);
            newEnemy.GetComponent<EnemyMovement>().OnChasedTarget += OnEnemyDeactivate;
            newEnemy.GetComponent<EnemyDeath>().OnHappened += OnEnemySlayed;
            _enemyQueue.Enqueue(newEnemy);

            return newEnemy;
        }

        public void Reload()
        {
            foreach (var enemy in _enemyQueue)
            {
                enemy.SetActive(false);

                foreach (var resettable in enemy.GetComponents<IResettable>())
                {
                    resettable.ResetState();
                }
            }
        }

        private void CreatePool(int poolSize)
        {
            _enemyQueue = new Queue<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                GameObject enemy = Instantiate(_enemyPrefab, transform);
                enemy.GetComponent<EnemyMovement>().SetChaseTarget(_target);
                enemy.GetComponent<EnemyMovement>().OnChasedTarget += OnEnemyDeactivate;
                enemy.GetComponent<EnemyDeath>().OnHappened += OnEnemySlayed;
                enemy.SetActive(false);
                _enemyQueue.Enqueue(enemy);
            }
        }

        private void OnEnemyDeactivate(GameObject obj)
        {
            OnChasedTarget?.Invoke();
            ReturnEnemy(obj);
        }

        private void OnEnemySlayed(GameObject obj)
        {
            OnEnemySlayedEvent?.Invoke();
            OnEnemyDeactivate(obj);
        }

        private void ReturnEnemy(GameObject enemy)
        {
            enemy.SetActive(false);

            foreach (var resettable in enemy.GetComponents<IResettable>())
            {
                resettable.ResetState();
            }

            _enemyQueue.Enqueue(enemy);
        }
    }
}