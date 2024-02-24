using UnityEngine;

namespace Code.Services.InputService
{
    public interface IInput : IService
    {
        Vector2 MoveAxis { get; }
        bool IsAttack { get; }
    }
}