using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class DistanceProgressBar : MonoBehaviour
    {
        [SerializeField] private Slider _progressSlider;

        private float _totalDistanceToCover;

        public void Construct(float totalDistanceToCover)
        {
            _totalDistanceToCover = totalDistanceToCover;
        }

        public void UpdateProgressBar(float distanceTraveled)
        {
            _progressSlider.value = distanceTraveled / _totalDistanceToCover;
        }

        public void Restart()
        {
            _progressSlider.value = 0;
        }
    }
}