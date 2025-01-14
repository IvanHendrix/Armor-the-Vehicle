using System;

namespace CodeBase.Services.GameplayControl
{
    public interface IGameplayControlService : IService
    {
        event Action OnGameStart;
        event Action OnGameRestart;
        event Action OnGameWin;
        event Action OnGameLose;
        event Action OnGamePause;
        void SetGameStart();
        void SetGameRestart();
        void SetGameWin();
        void SetGameLose();
        void SetGamePause();
    }

    public class GameplayControlService : IGameplayControlService
    {
        public event Action OnGameStart;
        public event Action OnGameRestart;
        public event Action OnGamePause;
        public event Action OnGameWin;
        public event Action OnGameLose;

        public void SetGamePause()
        {
            OnGamePause?.Invoke();
        }
        
        public void SetGameWin()
        {
            OnGamePause?.Invoke();
            OnGameWin?.Invoke();
        }
        
        public void SetGameLose()
        {
            OnGamePause?.Invoke();
            OnGameLose?.Invoke();
        }
        
        public void SetGameStart()
        {
            OnGameStart?.Invoke();
        }

        public void SetGameRestart()
        {
            OnGameRestart?.Invoke();
        }
    }
}