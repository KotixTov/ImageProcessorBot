using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessorBot.Commands
{
    class ChangeStateToDefaultCommand : IChatCommand
    {
        private ChatStateMachine _stateMachine;
        private string _commandText;

        public string CommandText => _commandText;

        public ChangeStateToDefaultCommand(ChatStateMachine stateMachine, string commandText)
        {
            _stateMachine = stateMachine;
            _commandText = commandText;
        }

        public Task Execute()
        {
            _stateMachine.ChangeStateToDefault();
            return Task.CompletedTask;
        }
    }
}
