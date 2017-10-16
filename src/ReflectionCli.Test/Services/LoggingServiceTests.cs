using ReflectionCli.Lib;
using ReflectionCli.Lib.Enums;
using Xunit;

namespace ReflectionCli.Test.Services
{
    public class LoggingServiceTests
    {
        private ILoggingService _service;
        private TestFixture _fixture;

        public LoggingServiceTests()
        {
            _fixture = new TestFixture();
            _service = _fixture.LoggingService;
        }

        [Fact]
        public void LoggingService_LogInfo_CorrectVerbosityLevel()
        {
            string message = "Info";

            _service.SetVerbosity(Verbosity.Info);
            _service.LogInfo(message);

            Assert.Equal(_service.GetLastTextWrittenInConsole(), $"[INF] {message}");
        }

        [Fact]
        public void LoggingService_LogInfo_VerbosityLevelSetAboveInfo()
        {
            string message = "Info";

            _service.SetVerbosity(Verbosity.Error);
            _service.LogInfo(message);

            Assert.Equal(_service.GetLastTextWrittenInConsole(), string.Empty);
        }
    }
}
