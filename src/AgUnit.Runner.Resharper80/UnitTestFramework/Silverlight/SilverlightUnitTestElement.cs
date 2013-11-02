using System;
using System.Collections.Generic;
using System.Xml;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.TaskRunnerFramework;
using JetBrains.ReSharper.UnitTestFramework;
#if RS80
using JetBrains.ReSharper.UnitTestFramework.Strategy;
#endif

namespace AgUnit.Runner.Resharper61.UnitTestFramework.Silverlight
{
    public class SilverlightUnitTestElement : IUnitTestElement
    {
        public SilverlightUnitTestElement(IUnitTestProvider provider)
            : this(provider, Guid.NewGuid().ToString())
        { }

        private SilverlightUnitTestElement(IUnitTestProvider provider, string id)
        {
            Id = id;
            Provider = provider;
            Children = new List<IUnitTestElement>();
        }

        public string Id { get; private set; }

        public IUnitTestProvider Provider { get; private set; }

        public string Kind
        {
            get { return "Silverlight tests"; }
        }

        public IEnumerable<UnitTestElementCategory> Categories
        {
            get { return UnitTestElementCategory.Uncategorized; }
        }

        public string ExplicitReason
        {
            get { return null; }
        }

        private IUnitTestElement parent;
        public IUnitTestElement Parent
        {
            get { return parent; }
            set
            {
                if (parent != value)
                {
                    if (parent != null)
                        parent.Children.Remove(this);

                    if (value != null)
                        value.Children.Add(this);

                    parent = value;
                }
            }
        }

        public ICollection<IUnitTestElement> Children { get; private set; }

        public string ShortName
        {
            get { return Id; }
        }

        public bool Explicit
        {
            get { return false; }
        }

        public UnitTestElementState State { get; set; }

        public bool Equals(IUnitTestElement other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (!(other is SilverlightUnitTestElement)) return false;
            return Equals(other.Id, Id);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SilverlightUnitTestElement)) return false;
            return Equals((SilverlightUnitTestElement)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public IProject GetProject()
        {
            return null;
        }

#if RS70 || RS71 || RS80
        public string GetPresentation(IUnitTestElement parent = null)
#else
        public string GetPresentation()
#endif
        {
            return Id;
        }

        public UnitTestNamespace GetNamespace()
        {
            return null;
        }

        public UnitTestElementDisposition GetDisposition()
        {
            return UnitTestElementDisposition.InvalidDisposition;
        }

        public IDeclaredElement GetDeclaredElement()
        {
            return null;
        }

        public IEnumerable<IProjectFile> GetProjectFiles()
        {
            return new List<IProjectFile>();
        }

#if RS80
        public IUnitTestRunStrategy GetRunStrategy(IHostProvider hostProvider)
        {
            return new OutOfProcessUnitTestRunStrategy(SilverlightUnitTestProvider.GetTaskRunnerInfo());
        }
#endif

#if RS70 || RS71 || RS80
        public IList<UnitTestTask> GetTaskSequence(ICollection<IUnitTestElement> explicitElements, IUnitTestLaunch launch)
#else
        public IList<UnitTestTask> GetTaskSequence(IList<IUnitTestElement> explicitElements)
#endif
        {
            return new List<UnitTestTask>();
        }

        public void Serialize(XmlElement xmlElement)
        {
            xmlElement.SetAttribute("type", typeof(SilverlightUnitTestElement).Name);
            xmlElement.SetAttribute("id", Id);
        }

        public static bool CanDeserialize(XmlElement xmlElement)
        {
            return xmlElement.HasAttribute("type")
                && xmlElement.GetAttribute("type") == typeof(SilverlightUnitTestElement).Name;
        }

        public static IUnitTestElement Deserialize(XmlElement xmlElement, SilverlightUnitTestProvider provider)
        {
            var id = xmlElement.GetAttribute("id");

            return new SilverlightUnitTestElement(provider, id);
        }
    }
}