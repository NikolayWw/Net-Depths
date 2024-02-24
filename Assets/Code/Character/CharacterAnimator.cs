using UnityEngine;

namespace Code.Character
{
    public class CharacterAnimator : MonoBehaviour
    {
        private readonly int MoveBlendHash = Animator.StringToHash("Move");
        private readonly int AttackHash = Animator.StringToHash("Attack");
        [SerializeField] private Animator[] _animators;

        public void PlayAttack()
        {
            foreach (Animator animator in _animators)
                animator.Play(AttackHash);
        }

        public void UpdateIdle()
        {
            foreach (Animator animator in _animators)
                animator.SetFloat(MoveBlendHash, 0);
        }

        public void UpdateWalking()
        {
            foreach (Animator animator in _animators)
                animator.SetFloat(MoveBlendHash, 1);
        }
    }
}