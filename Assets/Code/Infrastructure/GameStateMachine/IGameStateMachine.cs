using Code.Services;

namespace Code.Infrastructure.GameStateMachine
{
    public interface IGameStateMachine : IService
    {
        void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>;

        void Enter<TState, TPayload1, TPayload2>(TPayload1 payload1, TPayload2 payload2) where TState : class, IPayloadState<TPayload1, TPayload2>;

        void Enter<TState>() where TState : class, IState;
    }
}