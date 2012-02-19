using AgUnit.Runner.Resharper61.TaskRunner.UnitTestRunner.Silverlight.Providers;
using AgUnit.Runner.Resharper61.Util;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace AgUnit.Runner.Resharper61.TaskRunner.UnitTestProvider.XUnit
{
    public class XUnitMethodTaskProvider : IMethodTaskProvider
    {
        public bool IsMethodTask(RemoteTask task)
        {
            return task.GetType().FullName == "XunitContrib.Runner.ReSharper.RemoteRunner.XunitTestMethodTask";
        }

        public string GetFullMethodName(RemoteTask task)
        {
            var typeName = task.GetProperty<string>("TypeName");
            var shortName = task.GetProperty<string>("ShortName");

            return string.Format("{0}.{1}", typeName, shortName);
        }
    }
}