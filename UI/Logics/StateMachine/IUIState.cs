using Cysharp.Threading.Tasks;

namespace Code.UI.Logics.StateMachine
{
    /// <summary>
    /// This Interface is only for Views
    /// </summary>
    public interface IUIState
    {
        UIStateType StateType { get; }
        UniTask OnEnter();
        UniTask OnExit();
    }
}