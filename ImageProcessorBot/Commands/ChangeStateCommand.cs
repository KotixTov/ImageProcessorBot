using ImageProcessorBot.States;
using System.Threading.Tasks;

namespace ImageProcessorBot.Commands
{
    class ChangeStateCommand : IChatCommand
    {
        string IChatCommand.CommandText { get => _commandText; }

        private ChatStateMachine _stateMachine;
        private ChatState _targetState;
        private string _commandText;

        public ChangeStateCommand(ChatStateMachine stateMachine, ChatState targetState, string commandText)
        {
            _stateMachine = stateMachine;
            _targetState = targetState;
            _commandText = commandText;
        }

        Task ICommand.Execute()
        {
            _stateMachine.ChangeState(_targetState);
            return Task.CompletedTask;
        }
    }
}
