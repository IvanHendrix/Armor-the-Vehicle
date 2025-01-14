using CodeBase.CameraLogic;
using CodeBase.Car;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.States.Enum;
using CodeBase.Services;
using CodeBase.Services.World;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class InitGameLevelState : IGameState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IGameFactory _gameFactory;

        public InitGameLevelState(IGameStateMachine gameStateMachine, IGameFactory gameFactory)
        {
            _gameStateMachine = gameStateMachine;
            _gameFactory = gameFactory;
        }

        public void Enter()
        {
            InitLevelData();
            CreateLevel();
            EnterLoadLevel();
        }

        private void InitLevelData()
        {
            LocalServices.Container.Single<IWorldStateService>().Load();
        }

        private void CreateLevel()
        {
            _gameFactory.CreateGameManager();
            
            InitPlayer();

            _gameFactory.CreateRoad();

            _gameFactory.CreateHud();
            
            _gameFactory.CreateEnemies();
        }

        private void InitPlayer()
        {
            GameObject car = _gameFactory.CreateCar();
            Camera.main.GetComponent<CameraFollow>().Follow(car);
        }

        public void Exit()
        {
        }

        private void EnterLoadLevel()
        {
            _gameStateMachine.SetState(GameStateEnum.Gameplay);
        }
    }
}