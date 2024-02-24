namespace Code.Infrastructure.GameStateMachine
{
    public interface IState : IExitable
    {
        void Enter();
    }

    public interface IPayloadState<TPayload> : IExitable
    {
        void Enter(TPayload payLoad1);
    }

    public interface IPayloadState<TPayload1, TPayload2> : IExitable
    {
        void Enter(TPayload1 payLoad1, TPayload2 payload2);
    }

    public interface IExitable
    {
        void Exit();
    }
}