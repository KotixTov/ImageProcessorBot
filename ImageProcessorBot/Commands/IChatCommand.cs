namespace ImageProcessorBot.Commands
{
    public interface IChatCommand : ICommand
    {
        public string CommandText { get;}
    }
}
