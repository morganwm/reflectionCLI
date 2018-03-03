using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReflectionCli.Lib.Enums;
using ReflectionCli.Test.HelperServices;
using Xunit;
using static ReflectionCli.extended.ScriptingCommandSet;

namespace ReflectionCli.Test.Services
{
    public class RunScriptTests
    {
        private readonly FakeLoggingService _loggingService;
        private readonly RunScript _runScript;

        public RunScriptTests()
        {
            _loggingService = new FakeLoggingService();
            _runScript = new RunScript(_loggingService);
        }

        [Fact]
        public void RunScript_SimpleAdditionScript_ShouldReturnCorrectSum()
        {
            _runScript.Run("2+2");

            var res = _loggingService.Records
                .Where(t => t.RecordType == RecordType.Result)
                .ToList();

            Assert.Equal(res.Count, 1);
            Assert.Equal(res.Single().Message, "4");
        }
    }
}
