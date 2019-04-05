// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
namespace Microsoft.Azure.WebJobs.Script.Metrics
{
    public interface IMetricsPublisher
    {
        void Publish(DateTime timeStampUtc, string metricName, long data);
    }
}
