using System;
using System.Collections.Generic;
using System.Linq;
using AgUnit.Runner.Resharper61.UnitTestFramework.Silverlight;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.TaskRunnerFramework;
using JetBrains.ReSharper.UnitTestFramework;

namespace AgUnit.Runner.Resharper61.UnitTestFramework.SilverlightPlatform
{
    public static class UnitTestSequenceExtensions
    {
        public static IProject GetSilverlightProject(this IList<UnitTestTask> sequence)
        {
            return sequence
                .Where(task => task.Element != null)
                .Select(task => task.Element.GetProject())
                .Where(project => project != null && project.PlatformID != null && project.PlatformID.Identifier == FrameworkIdentifier.Silverlight)
                .FirstOrDefault();
        }

        public static bool IsSilverlightSequence(this IList<UnitTestTask> sequence)
        {
            return sequence.GetSilverlightUnitTestTask() != null;
        }

        public static Version GetSilverlightPlatformVersion(this IList<UnitTestTask> sequence)
        {
            var silverlightUnitTestTask = sequence.GetSilverlightUnitTestTask();
            return silverlightUnitTestTask != null ? silverlightUnitTestTask.SilverlightPlatformVersion : null;
        }

        public static SilverlightUnitTestTask GetSilverlightUnitTestTask(this IList<UnitTestTask> sequence)
        {
            return sequence.Select(task => task.RemoteTask).FirstOrDefault() as SilverlightUnitTestTask;
        }

        public static void AddSilverlightUnitTestTask(this IList<UnitTestTask> sequence, IProject silverlightProject, UnitTestProviders providers)
        {
            var provider = providers.GetProvider(SilverlightUnitTestProvider.RunnerId);
            var element = new SilverlightUnitTestElement(provider);

            var remoteTask = new SilverlightUnitTestTask(silverlightProject.PlatformID.Version, silverlightProject.GetXapPath(), silverlightProject.GetDllPath());
            sequence.Insert(0, new UnitTestTask(element, remoteTask));
        }

        public static void RemoveAssemblyLoadTasks(this IList<UnitTestTask> sequence)
        {
            var assemblyLoadTasks = sequence.Where(t => t.RemoteTask is AssemblyLoadTask).ToArray();
            foreach (var assemblyLoadTask in assemblyLoadTasks)
            {
                sequence.Remove(assemblyLoadTask);
            }
        }
    }
}