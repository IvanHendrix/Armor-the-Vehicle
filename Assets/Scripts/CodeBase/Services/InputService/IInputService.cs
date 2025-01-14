
using UnityEngine;

namespace CodeBase.Services.InputService
{
    public interface IInputService : IService
    {
        event System.Action OnInputDetected;
        Vector3 GetInputPosition();
        bool IsInputActive();
        void Update();
    }
}