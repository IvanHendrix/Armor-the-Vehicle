using CodeBase.Services;
using CodeBase.Services.GameplayControl;
using CodeBase.Services.InputService;
using CodeBase.Services.World;
using UnityEngine;

namespace CodeBase.Logic
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private bool _gameStarted = false;
        [SerializeField] private bool _gameFinished = false;
        
        private void Start()
        {
            LocalServices.Container.Single<IGameplayControlService>().OnGameLose += OnGameLose;
            LocalServices.Container.Single<IGameplayControlService>().OnGameWin += OnGameWin;
            
            LocalServices.Container.Single<IInputService>().OnInputDetected += OnClickDetected;
        }

        private void OnClickDetected()
        {
            if (!_gameStarted)
            {
                StartGame();
            }
            else
            {
                if (_gameFinished)
                {
                    RestartGame();
                }
            }
        }

        private void StartGame()
        {
            _gameStarted = true;
            LocalServices.Container.Single<IGameplayControlService>().SetGameStart();
        }

        private void OnGameLose()
        {
            _gameFinished = true;
            Debug.Log("You Lose!");
        }

        private void OnGameWin()
        {
            _gameFinished = true;
            Debug.Log("You Win!");
        }

        private void RestartGame()
        {
            _gameStarted = false;
            _gameFinished = false;
            
            LocalServices.Container.Single<IWorldStateService>().CleanData();
            LocalServices.Container.Single<IGameplayControlService>().SetGameRestart();
        }
    }
}