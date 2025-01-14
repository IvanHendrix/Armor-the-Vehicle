using System;
using UnityEngine;

namespace CodeBase.Services.InputService
{
    public class MouseInputService : IInputService
    {
        public event Action OnInputDetected;

        public Vector3 GetInputPosition()
        {
            return Input.mousePosition;
        }

        public bool IsInputActive()
        {
            return Input.GetMouseButton(0);
        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnInputDetected?.Invoke();
            }
        }
    }
}