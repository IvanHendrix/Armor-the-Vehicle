using System.Collections;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.States.Enum;
using CodeBase.Services;
using CodeBase.Services.GameplayControl;
using CodeBase.Services.InputService;
using CodeBase.Services.World;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class GameInitState : IGameState
    {
        private const string MainSceneName = "Main";
        
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LocalServices _services;
        private readonly ICoroutineRunner _coroutineRunner;

        public GameInitState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
            LocalServices services, ICoroutineRunner coroutineRunner)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            _coroutineRunner = coroutineRunner;

            RegisterServices();
        }

        public void Enter()
        {
            _sceneLoader.Load(MainSceneName, EnterLoadLevel);
        }

        private void EnterLoadLevel()
        {
            _gameStateMachine.SetState(GameStateEnum.InitLevel);
        }

        public void Exit()
        {
        }

        private void RegisterServices()
        {
            _services.RegisterSingle<IGameStateMachine>(_gameStateMachine);
            _services.RegisterSingle<IGameFactory>(new GameFactory());
            _services.RegisterSingle<IWorldStateService>(new WorldStateService());
            _services.RegisterSingle<IGameplayControlService>(new GameplayControlService());
            
            InitInput();
        }

        private void InitInput()
        {
            _services.RegisterSingle<IInputService>(InputService());
            _coroutineRunner.StartCoroutine(InputDetectionCoroutine());
        }

        private IEnumerator InputDetectionCoroutine()
        {
            while (true)
            {
                _services.Single<IInputService>().Update();
                yield return null;  
            }
        }

        private IInputService InputService()
        {
            return Application.isEditor
                ? new MouseInputService()
                : new MobileInputService();
        }
    }
}