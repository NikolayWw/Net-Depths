using Code.Services.InputService;
using UnityEngine;

namespace Code.Enemy
{
    public class EnemyInput : MonoBehaviour, IInput
    {
        public Vector2 MoveAxis { get; }
        public bool IsAttack { get; private set; }

        public void SetAttack(bool isAttack) =>
            IsAttack = isAttack;
    }
}