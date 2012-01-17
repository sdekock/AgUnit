extern alias util;
using AgUnit.Runner.Resharper60.UnitTestFramework.SilverlightPlatform;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestExplorer;
using JetBrains.ReSharper.UnitTestFramework;

namespace AgUnit.Runner.Resharper60.UnitTestFramework
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

        public ITaskRunnerHostController CreateHostController(ISolution solution, IUnitTestSessionManager sessionManager, IUnitTestLaunch launch, string remotingAddress)
        {
            var providers = solution.GetComponent<UnitTestProviders>();
            launch.EnsureSilverlightPlatformSupport(providers);

            return CreateWrappedHostController(solution, sessionManager, launch, remotingAddress);
        }

        protected virtual ITaskRunnerHostController CreateWrappedHostController(ISolution solution, IUnitTestSessionManager sessionManager, IUnitTestLaunch launch, string remotingAddress)
        {
            return WrappedHostProvider.CreateHostController(solution, sessionManager, launch, remotingAddress);
        }
    }
}   