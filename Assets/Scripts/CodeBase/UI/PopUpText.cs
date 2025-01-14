using TMPro;
using UnityEngine;

namespace CodeBase.UI
{
    public class PopUpText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private void Start()
        {
            Destroy(gameObject, 2f);
        }

        public void SetContextData(string text, Color color)
        {
            _text.text = text;
            _text.color = color;
        }
    }
}