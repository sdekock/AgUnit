﻿using AgUnit.Runner.Resharper60.TaskRunner.UnitTestRunner.Silverlight.Providers;
using JetBrains.ReSharper.TaskRunnerFramework;
using JetBrains.ReSharper.UnitTestRunner.nUnit;

namespace AgUnit.Runner.Resharper60.TaskRunner.UnitTestProvider.nUnit
{
    public class NUnitAssemblyTaskProvider : IAssemblyTaskProvider
    {
        public bool IsAssemblyTask(RemoteTask task)
        {
            return task is NUnitTestAssemblyTask;
        }

        private NUnitTestAssemblyTask GetTask(RemoteTask task)
        {
            return (NUnitTestAssemblyTask)task;
        }

        public string GetAssemblyLocation(RemoteTask task)
        {
            var assemblyTask = GetTask(task);

            return assemblyTask.AssemblyLocation;
        }
    }
}