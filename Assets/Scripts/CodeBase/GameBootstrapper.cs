using CodeBase.Infrastructure.States.Enum;
using UnityEngine;

namespace CodeBase
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private Game _game;

        private void Awake()
        {
            _game = new Game(this);
            _game.StateMachine.SetState(GameStateEnum.Init);
            
            DontDestroyOnLoad(this);
        }
    }
}