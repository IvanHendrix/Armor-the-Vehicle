using CodeBase.Infrastructure.States.Enum;
using CodeBase.Services;

namespace CodeBase.Infrastructure.States
{
    public interface IGameStateMachine : IService
    {
        void SetState(GameStateEnum newState);
    }
}