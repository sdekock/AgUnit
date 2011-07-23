using JetBrains.ReSharper.TaskRunnerFramework;

namespace AgUnit.Runner.Resharper60.TaskRunner.UnitTestRunner.Silverlight.Providers
{
    public interface IMethodTaskProvider
    {
        bool IsMethodTask(RemoteTask task);
        string GetFullMethodName(RemoteTask task);
    }
}