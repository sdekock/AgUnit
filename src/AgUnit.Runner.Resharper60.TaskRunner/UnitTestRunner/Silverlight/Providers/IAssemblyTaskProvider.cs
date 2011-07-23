using JetBrains.ReSharper.TaskRunnerFramework;

namespace AgUnit.Runner.Resharper60.TaskRunner.UnitTestRunner.Silverlight.Providers
{
    public interface IAssemblyTaskProvider
    {
        bool IsAssemblyTask(RemoteTask task);
        string GetXapPath(RemoteTask task);
    }
}