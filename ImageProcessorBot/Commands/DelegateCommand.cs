using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessorBot.Commands
{
    class DelegateCommand : IChatCommand
    {
        public delegate void operation();
        private operation _delegate;
        private string _commandText;

        public DelegateCommand(operation @delegate, string commandText)
        {
            _delegate = @delegate;
            _commandText = commandText;
        }

        public string CommandText => _commandText;

        public Task Execute()
        {
            _delegate?.Invoke();
            return Task.CompletedTask;
        }
    }
}
