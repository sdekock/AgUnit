using System;
using System.Collections.Generic;
using System.Linq;
using AgUnit.Runner.Resharper80.UnitTestFramework.Silverlight;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.ReSharper.UnitTestFramework.Strategy;

namespace AgUnit.Runner.Resharper80.UnitTestFramework.SilverlightPlatform
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

        public static void AddSilverlightUnitTestTask(this IList<UnitTestTask> sequence, IProject silverlightProject, UnitTestProviders providers, IUnitTestRunStrategy runStrategy)
        {
            var provider = providers.GetProvider(SilverlightUnitTestProvider.RunnerId);
            var element = new SilverlightUnitTestElement(provider, runStrategy);

            var remoteTask = new SilverlightUnitTestTask(silverlightProject.PlatformID.Version, silverlightProject.GetXapPath(), silverlightProject.GetDllPath());
            sequence.Insert(0, new UnitTestTask(element, remoteTask));
        }
    }
}