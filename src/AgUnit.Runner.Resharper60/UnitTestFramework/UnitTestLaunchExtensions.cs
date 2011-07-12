using System;
using System.Collections.Generic;
using System.Linq;
using AgUnit.Runner.Resharper60.UnitTestFramework.Silverlight;
using AgUnit.Runner.Resharper60.Util;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.TaskRunnerFramework;
using JetBrains.ReSharper.UnitTestExplorer;
using JetBrains.ReSharper.UnitTestFramework;
using PlatformID = JetBrains.ProjectModel.PlatformID;

namespace AgUnit.Runner.Resharper60.UnitTestFramework
{
    public static class UnitTestLaunchExtensions
    {
        public static void EnsureSilverlightPlatformSupport(this IUnitTestLaunch launch, UnitTestManager manager)
        {
            var runs = GetRuns(launch);

            foreach (var run in runs.Values.ToArray())
            {
                CheckForSilverlightSequences(launch, runs, run, manager);
            }

            RemoveEmptyRuns(runs);
        }

        private static Dictionary<string, UnitTestRun> GetRuns(IUnitTestLaunch launch)
        {
            return launch.GetField<Dictionary<string, UnitTestRun>>("myRuns");
        }

        private static IList<IList<UnitTestTask>> GetSequences(UnitTestRun run)
        {
            return run.GetField<IList<IList<UnitTestTask>>>("mySequences");
        }

        private static void RemoveEmptyRuns(Dictionary<string, UnitTestRun> runs)
        {
            var emptyRuns = runs.Values.Where(run => !GetSequences(run).Any()).ToArray();

            foreach (var run in emptyRuns)
            {
                runs.Remove(run.ID);
            }
        }

        private static void CheckForSilverlightSequences(IUnitTestLaunch launch, Dictionary<string, UnitTestRun> runs, UnitTestRun run, UnitTestManager manager)
        {
            var silverlightSequenceFound = false;
            var sequences = GetSequences(run);

            foreach (var sequence in sequences.ToArray())
            {
                if (!IsAlreadySilverlightSequence(sequence))
                {
                    var silverlightPlatform = GetSilverlightPlatform(sequence);

                    if (silverlightPlatform != null)
                    {
                        sequences.Remove(sequence);

                        silverlightSequenceFound = true;
                        CreateSilverlightSequence(sequence, silverlightPlatform, launch, runs);
                    }
                }
            }

            if (silverlightSequenceFound)
            {
                AddSilverlightUnitTestTask(manager, sequences);
            }
        }

        // We do this to make sure the SilverlightUnitTestProvider is passed to the runner, so the assemblies are loaded.
        private static void AddSilverlightUnitTestTask(UnitTestManager manager, IList<IList<UnitTestTask>> sequences)
        {
            var provider = manager.GetProvider(SilverlightUnitTestProvider.RunnerId);
            var element = new SilverlightUnitTestElement(provider);
            var remoteTask = new SilverlightUnitTestTask((Version)null);
            var task = new UnitTestTask(element, remoteTask);

            sequences.Add(new List<UnitTestTask> { task });
        }

        private static bool IsAlreadySilverlightSequence(IEnumerable<UnitTestTask> sequence)
        {
            return sequence.Select(task => task.RemoteTask).FirstOrDefault() is SilverlightUnitTestTask;
        }

        private static PlatformID GetSilverlightPlatform(IEnumerable<UnitTestTask> sequence)
        {
            return sequence.Where(task => task.Element != null)
                .Select(task => task.Element.GetProject())
                .Where(project => project != null)
                .Select(project => project.PlatformID)
                .FirstOrDefault(platform => platform != null && platform.Identifier == FrameworkIdentifier.Silverlight);
        }

        private static void CreateSilverlightSequence(IList<UnitTestTask> sequence, PlatformID silverlightPlatform, IUnitTestLaunch launch, Dictionary<string, UnitTestRun> runs)
        {
            AddSilverlightUnitTestTask(sequence, silverlightPlatform);
            RemoveAssemblyLoadTasks(sequence);

            var silverlightRun = GetOrCreateRun(launch, runs);
            silverlightRun.AddTaskSequence(sequence);
        }

        private static void AddSilverlightUnitTestTask(IList<UnitTestTask> sequence, PlatformID silverlightPlatform)
        {
            var remoteTask = new SilverlightUnitTestTask(silverlightPlatform.Version);

            sequence.Insert(0, new UnitTestTask(null, remoteTask));
        }

        private static void RemoveAssemblyLoadTasks(IList<UnitTestTask> sequence)
        {
            var assemblyLoadTasks = sequence.Where(t => t.RemoteTask is AssemblyLoadTask).ToArray();
            foreach (var assemblyLoadTask in assemblyLoadTasks)
            {
                sequence.Remove(assemblyLoadTask);
            }
        }

        private static UnitTestRun GetOrCreateRun(IUnitTestLaunch launch, Dictionary<string, UnitTestRun> runs)
        {
            var runtimeEnvironment = new RuntimeEnvironment { PlatformType = PlatformType.x86, PlatformVersion = PlatformVersion.v4_0 };
            var run = runs.Values.FirstOrDefault(r => Equals(r.RuntimeEnvironment, runtimeEnvironment));

            if (run == null)
            {
                run = new UnitTestRun((UnitTestLaunch)launch, runtimeEnvironment);
                runs.Add(run.ID, run);
            }

            return run;
        }
    }
}