extern alias util;
extern alias mstestlegacy;
extern alias mstest10;

using System.Collections.Generic;
using System.Threading;
using System.Linq;
using AgUnit.Runner.Resharper80.UnitTestFramework.Silverlight;
using JetBrains.Application.Settings;
using util::JetBrains.DataFlow;
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using JetBrains.Application;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.ReSharper.UnitTestFramework.Elements;

namespace AgUnit.Runner.Resharper80.UnitTestProvider.MSTest
{
    [MetadataUnitTestExplorer]
    public class SilverlightMsTestMetadataExplorer : IUnitTestMetadataExplorer
    {
        private const string SilverlightMsTestAssemblyName = "Microsoft.VisualStudio.QualityTools.UnitTesting.Silverlight";

        private readonly IShellLocks shellLocks;
        private readonly ISettingsStore settingsStore;
        private readonly Lifetime lifetime;
        private readonly ISolution solution;
        private readonly IUnitTestElementManager unitTestElementManager;
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

        public SilverlightMsTestMetadataExplorer(IShellLocks shellLocks, ISettingsStore settingsStore, SilverlightUnitTestProvider provider, Lifetime lifetime, ISolution solution,
            IUnitTestElementManager unitTestElementManager, mstestlegacy::JetBrains.ReSharper.UnitTestProvider.MSTest.IMsTestElementFactory elementFactory)
        {
            Provider = provider;
            this.shellLocks = shellLocks;
            this.settingsStore = settingsStore;
            this.lifetime = lifetime;
            this.solution = solution;
            this.unitTestElementManager = unitTestElementManager;
            this.elementFactory = elementFactory;
        }

        private mstest10::JetBrains.ReSharper.UnitTestProvider.MSTest10.MsTestElementFactory CreateMsTestElementFactory()
        {
            var msTestProvider = new mstest10::JetBrains.ReSharper.UnitTestProvider.MSTest10.MsTestProvider(settingsStore, MsTestAttributesProvider);
            var msTestServices = new mstest10::JetBrains.ReSharper.UnitTestProvider.MSTest10.MsTestServices(lifetime, solution, msTestProvider, settingsStore);

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