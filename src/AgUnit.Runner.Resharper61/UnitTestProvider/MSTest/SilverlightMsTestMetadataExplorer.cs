#if RS80

extern alias util;
extern alias mstestlegacy;
extern alias mstest10;
extern alias mstest11;

using System.Collections.Generic;
using System.Threading;
using System.Linq;
using AgUnit.Runner.Resharper61.UnitTestFramework.Silverlight;
using JetBrains.Application.Settings;
using util::JetBrains.DataFlow;
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using JetBrains.Application;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.ReSharper.UnitTestFramework.Elements;

namespace AgUnit.Runner.Resharper61.UnitTestProvider.MSTest
{
    [MetadataUnitTestExplorer]
    public class SilverlightMsTestMetadataExplorer : IUnitTestMetadataExplorer
    {
        private const string SilverlightMsTestAssemblyName = "Microsoft.VisualStudio.QualityTools.UnitTesting.Silverlight";

        private readonly IShellLocks shellLocks;
        private readonly ISettingsStore settingsStore;
        private readonly Lifetime lifetime;
        private readonly ISolution solution;
        private readonly mstestlegacy::JetBrains.ReSharper.UnitTestProvider.MSTest.IMsTestElementFactory elementFactory;

        public IUnitTestProvider Provider { get; private set; }

        private mstest10::JetBrains.ReSharper.UnitTestProvider.MSTest10.MsTestAttributesProvider msTestAttributesProvider;
        private mstest10::JetBrains.ReSharper.UnitTestProvider.MSTest10.MsTestAttributesProvider MsTestAttributesProvider
        {
            get { return msTestAttributesProvider ?? (msTestAttributesProvider = new mstest10::JetBrains.ReSharper.UnitTestProvider.MSTest10.MsTestAttributesProvider()); }
        }

        private mstest10::JetBrains.ReSharper.UnitTestProvider.MSTest10.MsTestElementFactory msTestElementFactory;
        private mstest10::JetBrains.ReSharper.UnitTestProvider.MSTest10.MsTestElementFactory MsTestElementFactory
        {
            get { return msTestElementFactory ?? (msTestElementFactory = CreateMsTestElementFactory()); }
        }

        public SilverlightMsTestMetadataExplorer(IShellLocks shellLocks, ISettingsStore settingsStore, SilverlightUnitTestProvider provider, Lifetime lifetime, ISolution solution, mstestlegacy::JetBrains.ReSharper.UnitTestProvider.MSTest.IMsTestElementFactory elementFactory)
        {
            Provider = provider;
            this.shellLocks = shellLocks;
            this.settingsStore = settingsStore;
            this.lifetime = lifetime;
            this.solution = solution;
            this.elementFactory = elementFactory;
        }

        private mstest10::JetBrains.ReSharper.UnitTestProvider.MSTest10.MsTestElementFactory CreateMsTestElementFactory()
        {
            var msTestProvider = new mstest10::JetBrains.ReSharper.UnitTestProvider.MSTest10.MsTestProvider(settingsStore, MsTestAttributesProvider);
            var msTestServices = new mstest10::JetBrains.ReSharper.UnitTestProvider.MSTest10.MsTestServices(lifetime, solution, msTestProvider, settingsStore);
            var unitTestElementCaches = Enumerable.Empty<IUnitTestElementCache>();
            var unitTestElementByProjectCache = new UnitTestElementByProjectCache();
            var unitTestResultManager = new UnitTestResultManager(lifetime);
            var unitTestElementManager = new UnitTestElementManager(lifetime, shellLocks, unitTestElementCaches, unitTestElementByProjectCache, unitTestResultManager);

            return new mstest10::JetBrains.ReSharper.UnitTestProvider.MSTest10.MsTestElementFactory(lifetime, msTestServices, unitTestElementManager, settingsStore, solution);
        }

        public void ExploreAssembly(IProject project, IMetadataAssembly assembly, UnitTestElementConsumer consumer, ManualResetEvent exitEvent)
        {
            var envoy = ProjectModelElementEnvoy.Create(project);
            if (assembly.ReferencedAssembliesNames.Any(n => n.Name == SilverlightMsTestAssemblyName))
            {
                var allElements = new List<IUnitTestElement>();
                var mappedElements = new Dictionary<IUnitTestElement, IUnitTestElement>();

                new mstestlegacy::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestMetadataExplorer(MsTestElementFactory, MsTestAttributesProvider, project, shellLocks, allElements.Add)
                    .ExploreAssembly(assembly);

                foreach (var classElement in allElements.OfType<mstest10::JetBrains.ReSharper.UnitTestProvider.MSTest10.MsTestTestClassElement>())
                    mappedElements.Add(classElement, elementFactory.GetOrCreateClassElement(classElement.TypeName, project, envoy));

                foreach (var methodElement in allElements.OfType<mstest10::JetBrains.ReSharper.UnitTestProvider.MSTest10.MsTestTestMethodElement>())
                    mappedElements.Add(methodElement, elementFactory.GetOrCreateMethodElement(methodElement.Id, project, (mstestlegacy::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestTestClassElementBase)mappedElements[methodElement.Parent], envoy, methodElement.TypeName));

                foreach (var rowElement in allElements.OfType<mstest10::JetBrains.ReSharper.UnitTestProvider.MSTest10.MsTestTestRowElement>())
                    mappedElements.Add(rowElement, elementFactory.GetOrCreateRowElement(rowElement.Id, project, (mstestlegacy::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestTestMethodElementBase)mappedElements[rowElement.Parent], envoy));

                foreach (var element in allElements)
                {
                    IUnitTestElement mappedElement;
                    if (mappedElements.TryGetValue(element, out mappedElement))
                        consumer(mappedElements[element]);
                    else
                        consumer(element);
                }
            }
        }
    }
}

#elif !RS70 && !RS71

extern alias mstestlegacy;
extern alias mstest10;
extern alias mstest11;

using System;
using System.Linq;
using AgUnit.Runner.Resharper61.Util;
using JetBrains.Application;
using JetBrains.Metadata.Reader.API;
using JetBrains.Metadata.Utils;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestFramework;

namespace AgUnit.Runner.Resharper61.UnitTestProvider.MSTest
{
    [MetadataUnitTestExplorer]
    public class SilverlightMsTestMetadataExplorer : IUnitTestMetadataExplorer
    {
        private const string DotNetMsTestAssemblyName = "Microsoft.VisualStudio.QualityTools.UnitTestFramework";
        private const string SilverlightMsTestAssemblyName = "Microsoft.VisualStudio.QualityTools.UnitTesting.Silverlight";

        private readonly IShellLocks shellLocks;
        private readonly mstestlegacy::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestProvider msTestLegacyProvider;
        private readonly mstestlegacy::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestElementFactory msTestLegacyElementFactory;
        private readonly mstest10::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestProvider msTest10Provider;
        private readonly mstest10::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestElementFactory msTest10ElementFactory;

        public IUnitTestProvider Provider
        {
            get
            {
                if (msTest10Provider as IUnitTestProvider != null)
                    return msTest10Provider as IUnitTestProvider;

                return msTestLegacyProvider;
            }
        }

        public SilverlightMsTestMetadataExplorer(IShellLocks shellLocks,
            mstestlegacy::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestProvider msTestLegacyProvider = null,
            mstestlegacy::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestElementFactory msTestLegacyElementFactory = null,
            mstest10::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestProvider msTest10Provider = null,
            mstest10::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestElementFactory msTest10ElementFactory = null)
        {
            this.shellLocks = shellLocks;
            this.msTestLegacyProvider = msTestLegacyProvider;
            this.msTestLegacyElementFactory = msTestLegacyElementFactory;
            this.msTest10Provider = msTest10Provider;
            this.msTest10ElementFactory = msTest10ElementFactory;
        }

        public void ExploreAssembly(IProject project, IMetadataAssembly assembly, UnitTestElementConsumer consumer)
        {
            if (assembly.ReferencedAssembliesNames.Any(n => n.Name == SilverlightMsTestAssemblyName))
            {
                var originalReferencedAssemblies = InjectMsTestIntoReferencedAssemblyNames(assembly);


                try
                {
                    if (msTest10Provider != null)
                    {
                        new mstest10::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestMetadataExplorer(msTest10ElementFactory, project, shellLocks, consumer).ExploreAssembly(assembly);
                    }
                    else if (msTestLegacyProvider != null)
                    {
                        new mstestlegacy::JetBrains.ReSharper.UnitTestProvider.MSTest.MsTestMetadataExplorer(msTestLegacyElementFactory, project, shellLocks, consumer).ExploreAssembly(assembly);
                    }

                }
                finally
                {
                    SetReferencedAssemblyNames(assembly, originalReferencedAssemblies);
                }
            }
        }

        private AssemblyNameInfo[] InjectMsTestIntoReferencedAssemblyNames(IMetadataAssembly assembly)
        {
            var referencedAssemblyNames = assembly.ReferencedAssembliesNames.ToArray();
            var updatedReferencedAssemblyNames = new AssemblyNameInfo[referencedAssemblyNames.Length + 1];

            Array.Copy(referencedAssemblyNames, updatedReferencedAssemblyNames, referencedAssemblyNames.Length);
            updatedReferencedAssemblyNames[referencedAssemblyNames.Length] = new AssemblyNameInfo(DotNetMsTestAssemblyName);

            SetReferencedAssemblyNames(assembly, updatedReferencedAssemblyNames);

            return referencedAssemblyNames;
        }

        private void SetReferencedAssemblyNames(IMetadataAssembly assembly, AssemblyNameInfo[] referencedAssemblyNames)
        {
            assembly.SetField("myReferencedAssembliesNames", referencedAssemblyNames);
        }
    }
}

#endif