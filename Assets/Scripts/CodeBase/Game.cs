using CodeBase.Infrastructure;
using CodeBase.Infrastructure.States;
using CodeBase.Services;

namespace CodeBase
{
    public class Game
    {
        public GameStateMachine StateMachine;
        
        public Game(ICoroutineRunner coroutineRunner)
        {
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner),LocalServices.Container, coroutineRunner);
        }
    }
}