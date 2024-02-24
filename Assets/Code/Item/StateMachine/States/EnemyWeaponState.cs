using UnityEngine;

namespace Code.Item.StateMachine.States
{
    public class EnemyWeaponState : MonoBehaviour, IInHandExitState
    {
        public void OnStart()
        { }

        public void OnUpdate()
        { }

        public bool ExitCondition() =>
            true;
        public void Enter()
        {

        }
        public void Exit()
        { }
    }
}