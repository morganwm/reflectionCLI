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
    public class ExitTests
    {
        private readonly FakeLoggingService _loggingService;
        private readonly Exit _exit;

        public ExitTests()
        {
            _loggingService = new FakeLoggingService();
            _exit = new Exit(_loggingService);
        }

        [Fact]
        public void Exit_ExitFunction_ShouldSetProgramExitToTrue()
        {
            _exit.Run();
            
            Assert.True(Program.ShutDown);
        }

        [Fact]
        public void Exit_ExitFunction_ShouldPrintExitText()
        {
            _exit.Run();

            var res = _loggingService.Records
                .Where(t => t.RecordType == RecordType.Result)
                .ToList();

            Assert.Equal(res.Count, 1);
            Assert.Equal(res.Single().Message, "Shutting down....");
        }
    }
}
