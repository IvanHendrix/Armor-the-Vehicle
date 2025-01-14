using System;
using UnityEngine;

namespace CodeBase.Services.InputService
{
    public class MobileInputService : IInputService
    {
        public event Action OnInputDetected;

        public Vector3 GetInputPosition()
        {
            return Input.GetTouch(0).position;
        }

        public bool IsInputActive()
        {
            return Input.touchCount > 0;
        }

        public void Update()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                OnInputDetected?.Invoke();
            }
        }
    }
}