using System;
using System.Xml;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace AgUnit.Runner.Resharper60.UnitTestFramework.Silverlight
{
    [Serializable]
    public class SilverlightUnitTestTask : RemoteTask, IEquatable<SilverlightUnitTestTask>
    {
        private const string SilverlightPlatformVersionKey = "SilverlightPlatformVersion";

        public Version SilverlightPlatformVersion { get; private set; }

        public override bool IsMeaningfulTask
        {
            get { return false; }
        }

        public SilverlightUnitTestTask(XmlElement element)
            : base(element)
        {
            var value = GetXmlAttribute(element, SilverlightPlatformVersionKey);
            SilverlightPlatformVersion = !string.IsNullOrEmpty(value) ? new Version(value) : null;
        }

        public SilverlightUnitTestTask(Version silverlightPlatformVersion)
            : base(SilverlightUnitTestProvider.RunnerId)
        {
            SilverlightPlatformVersion = silverlightPlatformVersion;
        }

        public override void SaveXml(XmlElement element)
        {
            base.SaveXml(element);
            SetXmlAttribute(element, SilverlightPlatformVersionKey, (SilverlightPlatformVersion ?? (object)string.Empty).ToString());
        }

        #region Equals

        public bool Equals(SilverlightUnitTestTask other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.SilverlightPlatformVersion, SilverlightPlatformVersion);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as SilverlightUnitTestTask);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (SilverlightPlatformVersion != null ? SilverlightPlatformVersion.GetHashCode() : 0);
            }
        }

        #endregion
    }
}