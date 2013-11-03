using System;
using System.Collections.Generic;
using System.Xml;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.ReSharper.UnitTestFramework.Strategy;

namespace AgUnit.Runner.Resharper80.UnitTestFramework.Silverlight
{
    public class SilverlightUnitTestElement : IUnitTestElement
    {
        private readonly IUnitTestRunStrategy runStrategy;

        public SilverlightUnitTestElement(IUnitTestProvider provider, IUnitTestRunStrategy runStrategy)
        {
            this.runStrategy = runStrategy;
            Id = Guid.NewGuid().ToString();
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

        public string GetPresentation(IUnitTestElement parent = null)
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

        public IUnitTestRunStrategy GetRunStrategy(IHostProvider hostProvider)
        {
            return runStrategy;
        }

        public IList<UnitTestTask> GetTaskSequence(ICollection<IUnitTestElement> explicitElements, IUnitTestLaunch launch)
        {
            return new List<UnitTestTask>();
        }
    }
}