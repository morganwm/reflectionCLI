using ReflectionCli.Lib;

namespace ReflectionCli.Test
{
    public class TestFixture
    {
        public IAssemblyService AssemblyService = new AssemblyService();
        public ILoggingService LoggingService = new LoggingService();
        public IVariableService VariableService = new VariableService();
    }
}