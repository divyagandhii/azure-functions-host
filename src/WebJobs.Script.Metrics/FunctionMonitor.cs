// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;

namespace Microsoft.Azure.WebJobs.Script.Metrics
{
    public class FunctionMonitor : IFunctionMonitor
    {
        private readonly IMetricsPublisher _metricsPublisher;
        private Process _process;
        private Timer _processMonitorTimer;

        public FunctionMonitor(HttpClient httpClient)
        {
            _metricsPublisher = new LinuxContainerMetricsPublisher(httpClient);
            Console.WriteLine("Function monitor initialized");
        }

        public void Start()
        {
            _process = Process.GetCurrentProcess();
            _processMonitorTimer = new Timer(OnProcessMonitorTimer, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(1000));
        }

        public void Stop()
        {
            _processMonitorTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void OnProcessMonitorTimer(object state)
        {
            var commitSizeBytes = _process.PrivateMemorySize64;
            if (commitSizeBytes != 0)
            {
                _metricsPublisher.Publish(DateTime.Now, "MemorySize", commitSizeBytes);
            }
        }
    }
}
