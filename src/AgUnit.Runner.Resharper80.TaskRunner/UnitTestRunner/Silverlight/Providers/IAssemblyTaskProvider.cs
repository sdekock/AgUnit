using JetBrains.ReSharper.TaskRunnerFramework;

namespace AgUnit.Runner.Resharper80.TaskRunner.UnitTestRunner.Silverlight.Providers
{
    public interface IAssemblyTaskProvider
    {
        bool IsAssemblyTask(RemoteTask task);
        string GetAssemblyLocation(RemoteTask task);
    }
}