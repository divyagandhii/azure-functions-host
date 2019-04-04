// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.Azure.WebJobs.Script.Metrics;

namespace Microsoft.Azure.WebJobs.Script.WebHost.Diagnostics
{
    public class LinuxContainerMetricsPublisher : IMetricsPublisher
    {
        public void Publish(DateTime timeStampUtc, string metricNamespace, string metricName, long data, bool skipIfZero = true)
        {
            if (data == 0 && skipIfZero)
            {
                return;
            }

            // TODO: log the event
            Console.WriteLine($"MDM {timeStampUtc} {metricNamespace} {metricName} {data}");
        }
    }
}
