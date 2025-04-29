using CodeBase.Enemy;
using CodeBase.Ground;
using CodeBase.Infrastructure.Assets;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        GameObject CreateCar();
        void CreateEnemies();
        void CreateRoad();
        void CreateGameManager();
        void CreateHud();
    }

    public class GameFactory : IGameFactory
    {
        private GameObject _carGameObject;

        public void CreateGameManager()
        {
            _carGameObject = InstantiatePrefab(AssetsAddress.GameManager, Vector3.zero);
        }
        
        public GameObject CreateCar()
        {
            _carGameObject = InstantiatePrefab(AssetsAddress.CarPath, Vector3.zero);
            return _carGameObject;
        }

        public void CreateRoad()
        {
            GameObject road = InstantiatePrefab(AssetsAddress.RoadPath, Vector3.zero);
            road.GetComponent<RoadLoop>().SetTarget(_carGameObject);
        }

        public void CreateHud()
        {
            GameObject hud = InstantiatePrefab(AssetsAddress.HudPath, Vector3.zero);
        }

        public void CreateEnemies()
        {
            GameObject spawner = InstantiatePrefab(AssetsAddress.EnemySpawner, Vector3.zero);
            spawner.GetComponent<EnemySpawner>().SetTarget(_carGameObject);
        }

        private GameObject InstantiatePrefab(string path, Vector3 at)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, at, Quaternion.identity);
        }
    }
}