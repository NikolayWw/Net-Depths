using UnityEngine;

namespace Code.Character
{
    public class CharacterMove : MonoBehaviour
    {
        [SerializeField] private CharacterAnimator _animation;

        public void UpdateMove(Vector2 moveAxis)
        {
            Move(moveAxis);
            Rotate(moveAxis);
            Animation(moveAxis);
        }

        private void Move(Vector2 moveAxis)
        {
            moveAxis *= Time.deltaTime * 5f;
            transform.position += new Vector3(moveAxis.x, moveAxis.y);
        }

        private void Rotate(Vector2 input)
        {
            Vector3 angles = transform.eulerAngles;

            if (input.x > 0)
                angles.y = 0;
            else if (input.x < 0)
                angles.y = 180;

            transform.eulerAngles = angles;
        }

        private void Animation(Vector2 moveAxis)
        {
            if (moveAxis == Vector2.zero)
                _animation.UpdateIdle();
            else
                _animation.UpdateWalking();
        }
    }
}