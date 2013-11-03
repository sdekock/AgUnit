using AgUnit.Runner.Resharper80.TaskRunner.UnitTestRunner.Silverlight.Providers;
using AgUnit.Runner.Resharper80.Util;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace AgUnit.Runner.Resharper80.TaskRunner.UnitTestProvider.MSTest
{
    public class MsTestMethodTaskProvider : IMethodTaskProvider
    {
        public bool IsMethodTask(RemoteTask task)
        {
            return task.GetType().Name == "MsTestTestMethodTask";
        }

        public string GetFullMethodName(RemoteTask task)
        {
            return string.Format("{0}.{1}", task.GetProperty<string>("TypeName"), task.GetProperty<string>("ShortName"));
        }
    }
}