using AgUnit.Runner.Resharper61.TaskRunner.UnitTestRunner.Silverlight.Providers;
using AgUnit.Runner.Resharper61.Util;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace AgUnit.Runner.Resharper61.TaskRunner.UnitTestProvider.XUnit
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