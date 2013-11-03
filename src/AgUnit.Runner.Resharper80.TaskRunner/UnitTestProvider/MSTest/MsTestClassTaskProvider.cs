using AgUnit.Runner.Resharper80.TaskRunner.UnitTestRunner.Silverlight.Providers;
using AgUnit.Runner.Resharper80.Util;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace AgUnit.Runner.Resharper80.TaskRunner.UnitTestProvider.MSTest
{
    public class MsTestClassTaskProvider : IClassTaskProvider
    {
        public bool IsClassTask(RemoteTask task)
        {
            return task.GetType().Name == "MsTestTestClassTask";
        }

        public string GetFullClassName(RemoteTask task)
        {
            return task.GetProperty<string>("TypeName");
        }
    }
}