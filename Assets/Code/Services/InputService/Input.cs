using UnityEngine;

namespace Code.Services.InputService
{
    public class Input : IInput
    {
        public Vector2 MoveAxis => new(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"));
        public bool IsAttack => UnityEngine.Input.GetKey(KeyCode.Space);
    }
}