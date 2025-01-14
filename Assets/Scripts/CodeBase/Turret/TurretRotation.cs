using CodeBase.Services;
using CodeBase.Services.GameplayControl;
using CodeBase.Services.InputService;
using UnityEngine;

namespace CodeBase.Turret
{
    public class TurretRotation : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 10f;

        private float _previousAngle = 0f;

        private bool _isRotating;
        
        private void Start()
        {
            LocalServices.Container.Single<IGameplayControlService>().OnGameRestart += OnRestart;
            LocalServices.Container.Single<IGameplayControlService>().OnGameStart += OnInit;
            LocalServices.Container.Single<IGameplayControlService>().OnGamePause += OnPause;
            
        }

        private void Update()
        {
            if (!_isRotating)
            {
                return;
            }
            
            RotateTowardsMouse();
        }

        private void OnPause()
        {
            _isRotating = false;
        }

        private void RotateTowardsMouse()
        {
            Vector3 screenPosition = LocalServices.Container.Single<IInputService>().GetInputPosition();
            
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            Plane plane = new Plane(Vector3.up, transform.position);
            float distance;

            if (plane.Raycast(ray, out distance))
            {
                Vector3 pointInWorld = ray.GetPoint(distance);

                Vector3 direction = pointInWorld - transform.position;
                
                direction.y = 0;

                float angle = Vector3.Angle(transform.forward, direction);

                if (angle > 90)
                {
                    return;
                }
                
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
            }
        }
        
        private void OnInit()
        {
            _isRotating = true;
        }

        private void OnRestart()
        {
            _isRotating = false;
            _previousAngle = 0;
        }
    }
}