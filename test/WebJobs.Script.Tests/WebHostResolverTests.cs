﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Script.Config;
using Microsoft.Azure.WebJobs.Script.Eventing;
using Microsoft.Azure.WebJobs.Script.Metrics;
using Microsoft.Azure.WebJobs.Script.WebHost;
using Microsoft.Azure.WebJobs.Script.WebHost.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.WebJobs.Script.Tests;
using Moq;
using Xunit;

namespace Microsoft.Azure.WebJobs.Script.Tests
{
    public class WebHostResolverTests
    {
        [Fact]
        public void GetScriptHostConfiguration_SetsHostId()
        {
            var environmentSettings = new Dictionary<string, string>
            {
                { EnvironmentSettingNames.AzureWebsiteName, "testsite" },
                { EnvironmentSettingNames.AzureWebsiteSlotName, "production" },
            };

            using (var environment = new TestScopedEnvironmentVariable(environmentSettings))
            {
                var settingsManager = new ScriptSettingsManager();
                var secretManagerFactoryMock = new Mock<ISecretManagerFactory>();
                var eventManagerMock = new Mock<IScriptEventManager>();
                var routerMock = new Mock<IWebJobsRouter>();
                var settings = new WebHostSettings
                {
                    ScriptPath = @"c:\some\path",
                    LogPath = @"c:\log\path",
                    SecretsPath = @"c:\secrets\path"
                };

                Mock<IEventGenerator> eventGeneratorMock = new Mock<IEventGenerator>();
                Mock<IFunctionMonitor> functionMonitorMock = new Mock<IFunctionMonitor>();
                var resolver = new WebHostResolver(settingsManager, secretManagerFactoryMock.Object, eventManagerMock.Object, settings, routerMock.Object,
                    new TestLoggerProviderFactory(null), NullLoggerFactory.Instance, eventGeneratorMock.Object, functionMonitorMock.Object);

                ScriptHostConfiguration configuration = resolver.GetScriptHostConfiguration(settings);

                Assert.Equal("testsite", configuration.HostConfig.HostId);
            }
        }

        [Fact]
        public void CreateScriptHostConfiguration_StandbyMode_ReturnsExpectedConfiguration()
        {
            var settings = new WebHostSettings
            {
                IsSelfHost = true,
                ScriptPath = Path.Combine(Path.GetTempPath(), "Functions", "Standby")
            };

            var config = WebHostResolver.CreateScriptHostConfiguration(settings, true);

            Assert.Equal(FileLoggingMode.DebugOnly, config.FileLoggingMode);
            Assert.Null(config.HostConfig.StorageConnectionString);
            Assert.Null(config.HostConfig.DashboardConnectionString);
        }
    }
}
