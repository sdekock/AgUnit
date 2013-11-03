using AgUnit.Runner.Resharper80.TaskRunner.UnitTestRunner.Silverlight.Providers;
using AgUnit.Runner.Resharper80.Util;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace AgUnit.Runner.Resharper80.TaskRunner.UnitTestProvider.XUnit
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