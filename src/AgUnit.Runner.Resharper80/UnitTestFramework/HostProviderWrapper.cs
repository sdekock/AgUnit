extern alias util;
using AgUnit.Runner.Resharper80.UnitTestFramework.SilverlightPlatform;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestFramework;

namespace AgUnit.Runner.Resharper80.UnitTestFramework
{
    public class HostProviderWrapper<TWrappedHostProvider> : IHostProvider
        where TWrappedHostProvider : IHostProvider
    {
        public TWrappedHostProvider WrappedHostProvider { get; private set; }

        public HostProviderWrapper(TWrappedHostProvider wrappedHostProvider)
        {
            WrappedHostProvider = wrappedHostProvider;
        }

        public string ID
        {
            get { return WrappedHostProvider.ID; }
        }

        public IHostProviderDescriptor Descriptor
        {
            get { return WrappedHostProvider.Descriptor; }
        }

        public bool Available()
        {
            return WrappedHostProvider.Available();
        }

        public bool Available(IUnitTestElement element)
        {
            return WrappedHostProvider.Available(element);
        }

        public ITaskRunnerHostController CreateHostController(ISolution solution, IUnitTestLaunchManager launchManager, IUnitTestAgentManager agentManager, IUnitTestLaunch launch)
        {
            var hostController = CreateWrappedHostController(solution, launchManager, agentManager, launch);

            var providers = solution.GetComponent<UnitTestProviders>();
            launch.EnsureSilverlightPlatformSupport(providers, hostController);

            return hostController;
        }

        protected virtual ITaskRunnerHostController CreateWrappedHostController(ISolution solution, IUnitTestLaunchManager launchManager, IUnitTestAgentManager agentManager, IUnitTestLaunch launch)
        {
            return WrappedHostProvider.CreateHostController(solution, launchManager, agentManager, launch);
        }
    }
}   