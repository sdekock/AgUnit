using System;
using System.Xml;
using JetBrains.ReSharper.TaskRunnerFramework;

namespace AgUnit.Runner.Resharper61.UnitTestFramework.Silverlight
{
    [Serializable]
    public class SilverlightUnitTestTask : RemoteTask, IEquatable<SilverlightUnitTestTask>
    {
        private const string SilverlightPlatformVersionKey = "SilverlightPlatformVersion";
        private const string XapPathKey = "XapPath";
        private const string DllPathKey = "DllPath";

        public Version SilverlightPlatformVersion { get; private set; }
        public string XapPath { get; private set; }
        public string DllPath { get; private set; }

        public override bool IsMeaningfulTask
        {
            get { return false; }
        }

        public SilverlightUnitTestTask(XmlElement element)
            : base(element)
        {
            var silverlightPlatformVersion = GetXmlAttribute(element, SilverlightPlatformVersionKey);
            SilverlightPlatformVersion = !string.IsNullOrEmpty(silverlightPlatformVersion) ? new Version(silverlightPlatformVersion) : null;
            XapPath = GetXmlAttribute(element, XapPathKey);
            DllPath = GetXmlAttribute(element, DllPathKey);
        }

        public SilverlightUnitTestTask(Version silverlightPlatformVersion, string xapPath, string dllPath)
            : base(SilverlightUnitTestProvider.RunnerId)
        {
            SilverlightPlatformVersion = silverlightPlatformVersion;
            XapPath = xapPath;
            DllPath = dllPath;
        }

        public override void SaveXml(XmlElement element)
        {
            base.SaveXml(element);
            SetXmlAttribute(element, SilverlightPlatformVersionKey, (SilverlightPlatformVersion ?? (object)string.Empty).ToString());
            SetXmlAttribute(element, XapPathKey, XapPath);
            SetXmlAttribute(element, DllPathKey, DllPath);
        }

        #region Equals

        public bool Equals(SilverlightUnitTestTask other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.SilverlightPlatformVersion, SilverlightPlatformVersion) && Equals(other.XapPath, XapPath) && Equals(other.DllPath, DllPath);
        }

#if RS80
        public override bool Equals(RemoteTask other)
        {
            return Equals(other as object);
        }
#endif

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
#if RS80
                int result = 0;
#else
                int result = base.GetHashCode();
#endif
                result = (result*397) ^ (SilverlightPlatformVersion != null ? SilverlightPlatformVersion.GetHashCode() : 0);
                result = (result*397) ^ (XapPath != null ? XapPath.GetHashCode() : 0);
                result = (result*397) ^ (DllPath != null ? DllPath.GetHashCode() : 0);
                return result;
            }
        }

        #endregion
    }
}