using UnityEngine;

namespace CodeBase.CameraLogic
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private float _rotationAngleX;
        [SerializeField] private float _distance;
        [SerializeField] private float _offsetY;

        private Transform _following;

        private void LateUpdate()
        {
            if (_following == null)
            {
                return;
            }
            
            Vector3 targetPosition = _following.position;
            
            targetPosition.x = transform.position.x;
            targetPosition.y += _offsetY;
            targetPosition.z -= _distance;
            
            transform.position = targetPosition;
            
            Quaternion rotation = Quaternion.Euler(_rotationAngleX, 0, 0);
            transform.rotation = rotation;
        }

        public void Follow(GameObject following)
        {
            _following = following.transform;
        }
    }
}