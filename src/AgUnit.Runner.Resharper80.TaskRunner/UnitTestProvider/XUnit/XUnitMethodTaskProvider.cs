using System;
using AgUnit.Runner.Resharper80.TaskRunner.UnitTestRunner.Silverlight.Providers;
using AgUnit.Runner.Resharper80.Util;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace AgUnit.Runner.Resharper80.TaskRunner.UnitTestProvider.XUnit
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
            
            string methodName;
            
            try
            {
                methodName = task.GetProperty<string>("MethodName");
            }
            catch (Exception)
            {
                methodName = task.GetProperty<string>("ShortName");
            }

            return string.Format("{0}.{1}", typeName, methodName);
        }
    }
}