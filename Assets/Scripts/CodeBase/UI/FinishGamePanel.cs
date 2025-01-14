using TMPro;
using UnityEngine;

namespace CodeBase.UI
{
    public class FinishGamePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        
        public void SetText(string text)
        {
            _text.text = text;
        }
    }
}