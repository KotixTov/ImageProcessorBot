using System.Threading.Tasks;

namespace ImageProcessorBot.Commands
{
    public interface ICommand
    {
        public Task Execute();
    }
}
