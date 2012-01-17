using StatLight.Core.Common;
using StatLight.Core.Properties;

namespace AgUnit.Runner.Resharper60.TaskRunner.UnitTestRunner.Silverlight
{
    // TODO: Move this to StatLight
    public class DebugLogger : LoggerBase
    {
        private readonly Settings settings;

        public DebugLogger()
            : this(LogChatterLevels.Error | LogChatterLevels.Warning | LogChatterLevels.Information, Settings.Default)
        { }

        public DebugLogger(LogChatterLevels logChatterLevel)
            : this(logChatterLevel, Settings.Default)
        { }

        public DebugLogger(LogChatterLevels logChatterLevel, Settings settings)
            : base(logChatterLevel)
        {
            this.settings = settings;
        }

        public override void Information(string message)
        {
            if (ShouldLog(LogChatterLevels.Information))
                Write(message, "Information", false);
        }

        public override void Debug(string message)
        {
            if (ShouldLog(LogChatterLevels.Debug))
                Write(message, "Debug", true);
        }

        public override void Debug(string message, bool writeNewLine)
        {
            if (ShouldLog(LogChatterLevels.Debug))
                Write(message, "Debug", writeNewLine);
        }

        public override void Warning(string message)
        {
            if (ShouldLog(LogChatterLevels.Warning))
                Write(message, "Warning", false);
        }

        public override void Error(string message)
        {
            if (ShouldLog(LogChatterLevels.Error))
                Write(message, "Error", true);
        }

        private void Write(string message, string type, bool useNewLine)
        {
            message = string.Format("[{0}]: {1}", type, message);

            if (useNewLine)
            {
                System.Diagnostics.Debug.WriteLine(message);
            }
            else
            {
                System.Diagnostics.Debug.Write(message);
            }
        }
    }
}
