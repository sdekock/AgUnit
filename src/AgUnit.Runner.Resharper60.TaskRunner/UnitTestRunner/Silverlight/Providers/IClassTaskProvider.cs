using JetBrains.ReSharper.TaskRunnerFramework;

namespace AgUnit.Runner.Resharper60.TaskRunner.UnitTestRunner.Silverlight.Providers
{
    public interface IClassTaskProvider
    {
        bool IsClassTask(RemoteTask task);
        string GetFullClassName(RemoteTask task);
    }
}