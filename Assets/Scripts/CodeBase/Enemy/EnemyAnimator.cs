using UnityEngine;

namespace CodeBase.Enemy
{
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public void SetMovingState(bool isMoving)
        {
            _animator.SetBool("IsMoving", isMoving);
        }

        public void SetSpeedAnimation(float speed)
        {
            _animator.SetFloat("Speed", speed);
        }
    }
}