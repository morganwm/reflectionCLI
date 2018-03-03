using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReflectionCli.Test.HelperServices;
using Xunit;
using static ReflectionCli.extended.ScriptingCommandSet;

namespace ReflectionCli.Test.Services
{
    public class RunScriptsTest
    {
        private readonly FakeLoggingService _loggingService;
        private readonly RunScript _runScript;

        public RunScriptsTest()
        {
            _loggingService = new FakeLoggingService();
            _runScript = new RunScript(_loggingService);
        }

        [Fact]
        public void RunScript_SimpleAdditionScript_ShouldReturnCorrectSum()
        {
            _runScript.Run("2+2");

            var outputRes = _loggingService.Output;
            var res =_loggingService.Results;

            Assert.Equal(res.Count, 1);
            Assert.Equal(res.Single(), "4");
        }
    }
}
