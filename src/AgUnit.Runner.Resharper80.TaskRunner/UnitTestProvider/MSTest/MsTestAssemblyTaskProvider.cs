using AgUnit.Runner.Resharper80.TaskRunner.UnitTestRunner.Silverlight.Providers;
using AgUnit.Runner.Resharper80.Util;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace AgUnit.Runner.Resharper80.TaskRunner.UnitTestProvider.MSTest
{
    public class MsTestAssemblyTaskProvider : IAssemblyTaskProvider
    {
        public bool IsAssemblyTask(RemoteTask task)
        {
            return task.GetType().Name == "MsTestTestAssemblyTask";
        }

        public string GetAssemblyLocation(RemoteTask task)
        {
            return task.GetProperty<string>("AssemblyLocation");
        }
    }
}