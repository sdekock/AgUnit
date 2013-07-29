using System;
using System.Drawing;
using System.IO;
using System.Xml;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.TaskRunnerFramework;
using JetBrains.ReSharper.UnitTestFramework;
#if RS80
using System.Reflection;
#endif

namespace AgUnit.Runner.Resharper61.UnitTestFramework.Silverlight
{
    [UnitTestProvider]
    public class SilverlightUnitTestProvider : IUnitTestProvider
    {
        public const string RunnerId = "Silverlight";
        public const string RunnerTypeName = "AgUnit.Runner.Resharper61.TaskRunner.UnitTestRunner.Silverlight.SilverlightUnitTestTaskRunner";
        public static readonly string RunnerCodeBase = GetRunnerCodeBase();

        private static string GetRunnerCodeBase()
        {
            var pluginCodeBase = typeof(SilverlightUnitTestProvider).Assembly.CodeBase;
            var directory = Path.GetDirectoryName(pluginCodeBase);
            var filename = Path.GetFileNameWithoutExtension(pluginCodeBase) + ".TaskRunner.dll";

            return new Uri(Path.Combine(directory, filename)).LocalPath;
        }

        public string ID
        {
            get { return RunnerId; }
        }

        public string Name
        {
            get { return "Silverlight"; }
        }

        public Image Icon
        {
            get { return null; }
        }

        public ISolution Solution
        {
            get { return null; }
        }

#if RS80
        public static RemoteTaskRunnerInfo GetTaskRunnerInfo()
        {
            var runnerType = Assembly.LoadFrom(GetRunnerCodeBase()).GetType(RunnerTypeName);
            return new RemoteTaskRunnerInfo(RunnerId, runnerType);
        }
#else
        public RemoteTaskRunnerInfo GetTaskRunnerInfo()
        {
            return new RemoteTaskRunnerInfo(RunnerCodeBase, RunnerTypeName);
        }
#endif

        public void ExploreExternal(UnitTestElementConsumer consumer)
        {
        }

        public void ExploreSolution(ISolution solution, UnitTestElementConsumer consumer)
        {
        }

        public bool IsElementOfKind(IDeclaredElement declaredElement, UnitTestElementKind elementKind)
        {
            return false;
        }

        public bool IsElementOfKind(IUnitTestElement element, UnitTestElementKind elementKind)
        {
            return false;
        }

        public bool IsSupported(IHostProvider hostProvider)
        {
            return true;
        }

        public int CompareUnitTestElements(IUnitTestElement x, IUnitTestElement y)
        {
            return 0;
        }

        public void SerializeElement(XmlElement parent, IUnitTestElement element)
        {
            if (element is SilverlightUnitTestElement)
            {
                ((SilverlightUnitTestElement)element).Serialize(parent);
            }

            throw new ArgumentOutOfRangeException();
        }

        public IUnitTestElement DeserializeElement(XmlElement parent, IUnitTestElement parentElement)
        {
            if (SilverlightUnitTestElement.CanDeserialize(parent))
            {
                return SilverlightUnitTestElement.Deserialize(parent, this);
            }

            throw new ArgumentOutOfRangeException();
        }
    }
}