using AgUnit.Runner.Resharper61.TaskRunner.UnitTestRunner.Silverlight.Providers;
using AgUnit.Runner.Resharper61.Util;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace AgUnit.Runner.Resharper61.TaskRunner.UnitTestProvider.XUnit
{
    public class XUnitClassTaskProvider : IClassTaskProvider
    {
        public bool IsClassTask(RemoteTask task)
        {
            return task.GetType().FullName == "XunitContrib.Runner.ReSharper.RemoteRunner.XunitTestClassTask";
        }

        public string GetFullClassName(RemoteTask task)
        {
            return task.GetProperty<string>("TypeName");
        }
    }
}