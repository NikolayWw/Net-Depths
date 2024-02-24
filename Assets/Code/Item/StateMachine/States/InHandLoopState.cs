namespace Code.Item.StateMachine.States
{
    public class InHandLoopState : IInHandExitState
    {
        public void OnStart()
        { }

        public void OnUpdate()
        { }

        public bool ExitCondition() =>
            true;

        public void Exit()
        { }
    }
}