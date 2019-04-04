// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.Azure.WebJobs.Script.Metrics;

namespace Microsoft.Azure.WebJobs.Script.WebHost.Diagnostics
{
    public class LinuxContainerAnalyticsPublisher : IAnalyticsPublisher
    {
        public void WriteEvent(string siteName = null,
            string feature = null,
            string objectTypes = null,
            string objectNames = null,
            string dataKeys = null,
            string dataValues = null,
            string action = null,
            DateTime? actionTimeStamp = null,
            bool succeeded = true)
        {
            // TODO: write event
        }

        public void WriteError(int processId, string containerName, string message, string details)
        {
            // TODO: write error
        }
    }
}
