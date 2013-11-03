using AgUnit.Runner.Resharper80.TaskRunner.UnitTestRunner.Silverlight.Providers;
using AgUnit.Runner.Resharper80.Util;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace AgUnit.Runner.Resharper80.TaskRunner.UnitTestProvider.XUnit
{
    public class XUnitAssemblyTaskProvider : IAssemblyTaskProvider
    {
        public bool IsAssemblyTask(RemoteTask task)
        {
            return task.GetType().FullName == "XunitContrib.Runner.ReSharper.RemoteRunner.XunitTestAssemblyTask";
        }

        public string GetAssemblyLocation(RemoteTask task)
        {
            return task.GetProperty<string>("AssemblyLocation");
        }
    }
}