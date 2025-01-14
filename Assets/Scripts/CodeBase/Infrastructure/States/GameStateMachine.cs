using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.States.Enum;
using CodeBase.Services;

namespace CodeBase.Infrastructure.States
{
    public class GameStateMachine : IGameStateMachine
    {
        private IGameState _currentGameState;

        private readonly Dictionary<GameStateEnum, IGameState> _states;

        public GameStateMachine(SceneLoader sceneLoader, LocalServices services, ICoroutineRunner coroutineRunner)
        {
            _states = new Dictionary<GameStateEnum, IGameState>();

            _states.Add(GameStateEnum.Init, new GameInitState(this, sceneLoader, services, coroutineRunner));
            _states.Add(GameStateEnum.InitLevel,
                new InitGameLevelState(this, services.Single<IGameFactory>()));
            _states.Add(GameStateEnum.Gameplay, new GameplayState());
            _states.Add(GameStateEnum.Reload, new GameOverState());
        }

        public void SetState(GameStateEnum newState)
        {
            IGameState state = ChangeState(newState);
            state.Enter();
        }

        private IGameState ChangeState(GameStateEnum newState)
        {
            _currentGameState?.Exit();

            IGameState state = _states[newState];
            _currentGameState = state;

            return state;
        }
    }
}