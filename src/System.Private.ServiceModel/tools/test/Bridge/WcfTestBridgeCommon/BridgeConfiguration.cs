﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WcfTestBridgeCommon
{
    [Serializable]
    public class BridgeConfiguration
    {
        private static readonly string s_default_BridgeHost = "localhost";
        private static readonly int s_default_BridgePort = 44283;
        private static readonly int s_default_BridgeHttpPort = 8081;
        private static readonly int s_default_BridgeHttpsPort = 44285;
        private static readonly int s_default_BridgeTcpPort = 809;
        private static readonly int s_default_BridgeWebSocketPort = 8083;
        private static readonly int s_default_BridgeSecureWebSocketPort = 8084;
        private static readonly TimeSpan s_default_BridgeMaxIdleTimeSpan = TimeSpan.FromHours(24);

        // These property names must match the names used in TestProperties because
        // that is the set of name/value pairs from which this type is created.
        private const string BridgeResourceFolder_PropertyName = "BridgeResourceFolder";
        private const string BridgeHost_PropertyName = "BridgeHost";
        private const string BridgePort_PropertyName = "BridgePort";
        private const string BridgeHttpPort_PropertyName = "BridgeHttpPort";
        private const string BridgeHttpsPort_PropertyName = "BridgeHttpsPort";
        private const string BridgeTcpPort_PropertyName = "BridgeTcpPort";
        private const string BridgeWebSocketPort_PropertyName = "BridgeWebSocketPort";
        private const string BridgeSecureWebSocketPort_PropertyName = "BridgeSecureWebSocketPort";
        private const string TestRootCertificatePassword_PropertyName = "TestRootCertificatePassword";
        private const string TestRootCertificateValidityPeriod_PropertyName = "TestRootCertificateValidityPeriod";
        private const string BridgeMaxIdleTimeSpan_PropertyName = "BridgeMaxIdleTimeSpan";
        private const string UseFiddlerUrl_PropertyName = "UseFiddlerUrl";

        public string BridgeResourceFolder { get; set; }
        public string BridgeHost { get; set; }
        public int BridgePort { get; set; }
        public int BridgeHttpPort { get; set; }
        public int BridgeHttpsPort { get; set; }
        public int BridgeTcpPort { get; set; }
        public int BridgeWebSocketPort { get; set; }
        public int BridgeSecureWebSocketPort { get; set; }
        public string TestRootCertificatePassword { get; set; }
        public TimeSpan TestRootCertificateValidityPeriod { get; set; }
        public TimeSpan BridgeMaxIdleTimeSpan { get; set; }
        public bool UseFiddlerUrl { get; set; }

        public BridgeConfiguration()
        {
            BridgeHost = s_default_BridgeHost;
            BridgePort = s_default_BridgePort;
            BridgeHttpPort = s_default_BridgeHttpPort;
            BridgeHttpsPort = s_default_BridgeHttpsPort;
            BridgeTcpPort = s_default_BridgeTcpPort;
            BridgeWebSocketPort = s_default_BridgeWebSocketPort;
            BridgeSecureWebSocketPort = s_default_BridgeSecureWebSocketPort;
            BridgeMaxIdleTimeSpan = s_default_BridgeMaxIdleTimeSpan;
        }

        public BridgeConfiguration(Dictionary<string, string> properties) : this(new BridgeConfiguration(), properties)
        {
        }

        // This ctor accepts an existing BridgeConfiguration and a set of name/value pairs.
        // It will create a new BridgeConfiguration instance that is a clone of the existing
        // one and will overwrite any properties with corresponding entries found in the name/value pairs.
        public BridgeConfiguration(BridgeConfiguration configuration, Dictionary<string, string> properties)
        {
            BridgeResourceFolder = configuration.BridgeResourceFolder;
            BridgeHost = configuration.BridgeHost;
            BridgePort = configuration.BridgePort;
            BridgeHttpPort = configuration.BridgeHttpPort;
            BridgeHttpsPort = configuration.BridgeHttpsPort;
            BridgeTcpPort = configuration.BridgeTcpPort;
            BridgeWebSocketPort = configuration.BridgeWebSocketPort;
            BridgeSecureWebSocketPort = configuration.BridgeSecureWebSocketPort;
            TestRootCertificatePassword = configuration.TestRootCertificatePassword;
            TestRootCertificateValidityPeriod = configuration.TestRootCertificateValidityPeriod;
            BridgeMaxIdleTimeSpan = configuration.BridgeMaxIdleTimeSpan;
            UseFiddlerUrl = configuration.UseFiddlerUrl;

            if (properties != null)
            {
                string propertyValue = null;
                if (properties.TryGetValue(BridgeResourceFolder_PropertyName, out propertyValue))
                {
                    if (string.IsNullOrEmpty(propertyValue) || !Directory.Exists(propertyValue))
                    {
                        throw new ArgumentException(
                            String.Format("The BridgeResourceFolder '{0}' does not exist.", propertyValue),
                            BridgeResourceFolder_PropertyName);
                    }

                    BridgeResourceFolder = Path.GetFullPath(propertyValue);
                }

                if (properties.TryGetValue(BridgeHost_PropertyName, out propertyValue))
                {
                    BridgeHost = propertyValue;
                }

                if (properties.TryGetValue(TestRootCertificatePassword_PropertyName, out propertyValue))
                {
                    TestRootCertificatePassword = propertyValue;
                }

                TimeSpan validity;
                if (TryParseTimeSpanProperty(TestRootCertificateValidityPeriod_PropertyName, properties, out validity))
                {
                    TestRootCertificateValidityPeriod = validity;
                }

                int port;

                if (TryParseIntegerProperty(BridgePort_PropertyName, properties, out port))
                {
                    BridgePort = port;
                }

                if (TryParseIntegerProperty(BridgeHttpPort_PropertyName, properties, out port))
                {
                    BridgeHttpPort = port;
                }

                if (TryParseIntegerProperty(BridgeHttpsPort_PropertyName, properties, out port))
                {
                    BridgeHttpsPort = port;
                }

                if (TryParseIntegerProperty(BridgeTcpPort_PropertyName, properties, out port))
                {
                    BridgeTcpPort = port;
                }

                if (TryParseIntegerProperty(BridgeWebSocketPort_PropertyName, properties, out port))
                {
                    BridgeWebSocketPort = port;
                }

                if (TryParseIntegerProperty(BridgeSecureWebSocketPort_PropertyName, properties, out port))
                {
                    BridgeSecureWebSocketPort = port;
                }

                if (properties.TryGetValue(BridgeMaxIdleTimeSpan_PropertyName, out propertyValue))
                {
                    TimeSpan span;
                    if (!TimeSpan.TryParse(propertyValue, out span))
                    {
                        throw new ArgumentException(
                            String.Format("The BridgeMaxIdleTimeSpan value '{0}' is not a valid TimeSpan.", propertyValue),
                            BridgeMaxIdleTimeSpan_PropertyName);
                    }

                    BridgeMaxIdleTimeSpan = span;
                }

                if (properties.TryGetValue(UseFiddlerUrl_PropertyName, out propertyValue))
                {
                    bool boolValue = false;
                    if (!bool.TryParse(propertyValue, out boolValue))
                    {
                        throw new ArgumentException(
                            String.Format("The UseFiddlerUrl value '{0}' is not a valid boolean.", propertyValue),
                            UseFiddlerUrl_PropertyName);
                    }

                    UseFiddlerUrl = boolValue;
                }
            }
        }

        private static bool TryParseIntegerProperty(string propertyName, Dictionary<string, string> properties, out int result)
        {
            string propertyValue;
            result = 0;
            if (properties.TryGetValue(propertyName, out propertyValue))
            {
                if (!int.TryParse(propertyValue, out result))
                {
                    throw new ArgumentException(
                        String.Format("The {0} value '{1}' is not a valid integer.", propertyName, propertyValue),
                        propertyName);
                }
                return true;
            }
            return false;
        }

        private static bool TryParseTimeSpanProperty(string propertyName, Dictionary<string, string> properties, out TimeSpan result)
        {
            string propertyValue;
            result = TimeSpan.Zero;
            if (properties.TryGetValue(propertyName, out propertyValue))
            {
                if (!TimeSpan.TryParse(propertyValue, out result))
                {
                    throw new ArgumentException(
                        String.Format("The {0} value '{1}' is not a valid TimeSpan.", propertyName, propertyValue),
                        propertyName);
                }
                return true;
            }
            return false;
        }

        public Dictionary<string, string> ToDictionary()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            result[BridgeResourceFolder_PropertyName] = BridgeResourceFolder;
            result[BridgeHost_PropertyName] = BridgeHost;
            result[BridgePort_PropertyName] = BridgePort.ToString();
            result[BridgeHttpPort_PropertyName] = BridgeHttpPort.ToString();
            result[BridgeHttpsPort_PropertyName] = BridgeHttpsPort.ToString();
            result[BridgeTcpPort_PropertyName] = BridgeTcpPort.ToString();
            result[BridgeWebSocketPort_PropertyName] = BridgeWebSocketPort.ToString();
            result[BridgeSecureWebSocketPort_PropertyName] = BridgeSecureWebSocketPort.ToString();
            result[TestRootCertificatePassword_PropertyName] = TestRootCertificatePassword;
            result[TestRootCertificateValidityPeriod_PropertyName] = TestRootCertificateValidityPeriod.ToString();
            result[BridgeMaxIdleTimeSpan_PropertyName] = BridgeMaxIdleTimeSpan.ToString();
            result[UseFiddlerUrl_PropertyName] = UseFiddlerUrl.ToString();

            return result;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("  {0} : '{1}'{2}", BridgeResourceFolder_PropertyName, BridgeResourceFolder, Environment.NewLine)
              .AppendFormat("  {0} : '{1}'{2}", BridgeHost_PropertyName, BridgeHost, Environment.NewLine)
              .AppendFormat("  {0} : '{1}'{2}", BridgePort_PropertyName, BridgePort, Environment.NewLine)
              .AppendFormat("  {0} : '{1}'{2}", BridgeHttpPort_PropertyName, BridgeHttpPort, Environment.NewLine)
              .AppendFormat("  {0} : '{1}'{2}", BridgeHttpsPort_PropertyName, BridgeHttpsPort, Environment.NewLine)
              .AppendFormat("  {0} : '{1}'{2}", BridgeTcpPort_PropertyName, BridgeTcpPort, Environment.NewLine)
              .AppendFormat("  {0} : '{1}'{2}", BridgeWebSocketPort_PropertyName, BridgeWebSocketPort, Environment.NewLine)
              .AppendFormat("  {0} : '{1}'{2}", BridgeSecureWebSocketPort_PropertyName, BridgeSecureWebSocketPort, Environment.NewLine)
              .AppendFormat("  {0} : '{1}'{2}", TestRootCertificatePassword_PropertyName, TestRootCertificatePassword, Environment.NewLine)
              .AppendFormat("  {0} : '{1}'{2}", TestRootCertificateValidityPeriod_PropertyName, TestRootCertificateValidityPeriod, Environment.NewLine)
              .AppendFormat("  {0} : '{1}'{2}", BridgeMaxIdleTimeSpan_PropertyName, BridgeMaxIdleTimeSpan, Environment.NewLine)
              .AppendFormat("  {0} : '{1}'{2}", UseFiddlerUrl_PropertyName, UseFiddlerUrl, Environment.NewLine);
            return sb.ToString();
        }
    }
}
