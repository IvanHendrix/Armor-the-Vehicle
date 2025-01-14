using CodeBase.Services;
using CodeBase.Services.GameplayControl;
using CodeBase.Services.World;
using TMPro;
using UnityEngine;

namespace CodeBase.UI
{
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField] private DistanceProgressBar _distanceProgressBar;
        [SerializeField] private TextMeshProUGUI _coinText;
        [SerializeField] private FinishGamePanel _finishGamePanel;

        private void Start()
        {
            _finishGamePanel.gameObject.SetActive(false);
            
            LocalServices.Container.Single<IWorldStateService>().FinishedDistanceChanged += OnFinishedDistanceChanged;
            LocalServices.Container.Single<IWorldStateService>().CollectedChanged += OnCoinCollected;

            LocalServices.Container.Single<IGameplayControlService>().OnGameRestart += OnRestartUI;
            LocalServices.Container.Single<IGameplayControlService>().OnGameWin += OnShowWinMessage;
            LocalServices.Container.Single<IGameplayControlService>().OnGameLose += OnShowLoseMessage;
            
            _distanceProgressBar.Construct(LocalServices.Container.Single<IWorldStateService>().GetLevelData().DistanceToFinish);
        }

        private void OnRestartUI()
        {
            _finishGamePanel.gameObject.SetActive(false);
            _coinText.text = string.Empty;
            _distanceProgressBar.Restart();
        }

        private void OnCoinCollected(int coins)
        {
            _coinText.text = coins.ToString();
        }

        private void OnShowLoseMessage()
        {
            _finishGamePanel.gameObject.SetActive(true);
            _finishGamePanel.SetText("You lose");
        }

        private void OnShowWinMessage()
        {
            _finishGamePanel.gameObject.SetActive(true);
            _finishGamePanel.SetText("You win");
        }
        
        private void OnFinishedDistanceChanged(float value)
        {
            if (value >= LocalServices.Container.Single<IWorldStateService>().GetLevelData().DistanceToFinish)
            {
                LocalServices.Container.Single<IGameplayControlService>().SetGamePause();
                LocalServices.Container.Single<IGameplayControlService>().SetGameWin();
            }
            
            _distanceProgressBar.UpdateProgressBar(value);
        }
    }
}