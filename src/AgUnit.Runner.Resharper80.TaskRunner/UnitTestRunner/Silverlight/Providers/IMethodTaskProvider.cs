using JetBrains.ReSharper.TaskRunnerFramework;

namespace AgUnit.Runner.Resharper80.TaskRunner.UnitTestRunner.Silverlight.Providers
{
    public interface IMethodTaskProvider
    {
        bool IsMethodTask(RemoteTask task);
        string GetFullMethodName(RemoteTask task);
    }
}