using System;
using System.Linq;
using System.Reflection;
using AgUnit.Runner.Resharper61.UnitTestFramework.Silverlight;
using AgUnit.Runner.Resharper61.Util;
using JetBrains.Application;
using JetBrains.Metadata.Utils;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.UnitTestFramework;

namespace AgUnit.Runner.Resharper61.UnitTestProvider.XUnit
{
    // This is a modified version of the XunitTestFileExplorer from the XunitContrib project.
    // This re-enables the support for Silverlight tests.
    // See: http://xunitcontrib.codeplex.com/SourceControl/changeset/view/e5a69c7db68e#resharper%2fresharper60%2fxunitcontrib.runner.resharper.provider.6.0%2fXunitTestFileExplorer.cs

    [FileUnitTestExplorer]
    public class SilverlightXunitTestFileExplorer : IUnitTestFileExplorer
    {
        private const string ProviderId = "xUnit";

        private readonly IUnitTestProvider provider;
        private readonly bool xUnitInstalled = true;

        public SilverlightXunitTestFileExplorer(UnitTestProviders providers)
        {
            provider = providers.GetProvider(ProviderId);

            if (provider == null)
            {
                xUnitInstalled = false;
                provider = providers.GetProvider(SilverlightUnitTestProvider.RunnerId);
            }
        }

        public void ExploreFile(IFile psiFile, UnitTestElementLocationConsumer consumer, CheckForInterrupt interrupted)
        {
            if (!xUnitInstalled)
                return;

            if (provider == null)
                return;

            if (psiFile == null)
                throw new ArgumentNullException("psiFile");

            var project = psiFile.GetProject();
            if (project == null)
                return;

            if (project.GetAssemblyReferences().Any(IsSilverlightMscorlib))
                return;

            var fileExplorers = psiFile.GetProject().GetSolution().GetComponents<IUnitTestFileExplorer>();
            var xunitTestFileExplorer = fileExplorers.Single(e => e.GetType().FullName == "XunitContrib.Runner.ReSharper.UnitTestProvider.XunitTestFileExplorer");
            var unitTestElementFactory = xunitTestFileExplorer.GetField<object>("unitTestElementFactory");
            var xunitPsiFileExplorerType = xunitTestFileExplorer.GetType().Assembly.GetType("XunitContrib.Runner.ReSharper.UnitTestProvider.XunitPsiFileExplorer");
            var xunitPsiFileExplorer = (IRecursiveElementProcessor)Activator.CreateInstance(xunitPsiFileExplorerType,
                 provider, unitTestElementFactory, consumer, psiFile, interrupted);

            psiFile.ProcessDescendants(xunitPsiFileExplorer);
        }

        private static bool IsSilverlightMscorlib(IProjectToAssemblyReference reference)
        {
            var assemblyNameInfo = reference.ReferenceTarget.AssemblyName;
            if (assemblyNameInfo == null)
                return false;

            var publicKeyTokenBytes = assemblyNameInfo.GetPublicKeyToken();
            if (publicKeyTokenBytes == null)
                return false;

            var publicKeyToken = AssemblyNameExtensions.GetPublicKeyTokenString(publicKeyTokenBytes);

            // Not sure if this is the best way to do this, but the public key token for mscorlib on
            // the desktop if "b77a5c561934e089". On Silverlight, it's "7cec85d7bea7798e"
            return assemblyNameInfo.Name == "mscorlib" && publicKeyToken == "b77a5c561934e089";
        }

        public IUnitTestProvider Provider
        {
            get { return provider; }
        }
    }
}