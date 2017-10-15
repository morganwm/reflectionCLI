using ReflectionCli.Lib;

namespace ReflectionCli
{
    public class NullCommand : ICommand
    {
        public bool ExitVal()
        {
            return false;
        }
    }
}