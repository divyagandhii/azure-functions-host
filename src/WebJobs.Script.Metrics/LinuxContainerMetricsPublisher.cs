// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using Microsoft.Azure.WebJobs.Script.Metrics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.Azure.WebJobs.Script.WebHost.Diagnostics
{
    public class LinuxContainerMetricsPublisher : IMetricsPublisher
    {
        private readonly string _hostRequestUri;
        private Timer _metricsPublisherTimer;
        private BlockingCollection<FunctionMetric> _metrics;
        private HttpClient _httpClient;
        private const string _portNumber = "";

        public LinuxContainerMetricsPublisher(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _metricsPublisherTimer = new Timer(OnFunctionMetricsPublishTimer, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(30 * 100));
            _hostRequestUri = BuildRequestUri();
            _metrics = new BlockingCollection<FunctionMetric>(new ConcurrentQueue<FunctionMetric>());
        }

        public void Publish(DateTime timeStampUtc, string metricNamespace, string metricName, long data, bool skipIfZero = true)
        {
            if (data == 0 && skipIfZero)
            {
                return;
            }

            var metric = new FunctionMetric
            {
                TimestampUtc = timeStampUtc,
                MetricNamespace = metricNamespace,
                MetricName = metricName,
                MetricValue = data
            };

            _metrics.Add(metric);
        }

        private string BuildRequestUri()
        {
            var protocol = "https";
            var hostname = Environment.GetEnvironmentVariable("HOST_IP");
            var requestUri = $"{protocol}://{hostname}/{_portNumber}/api/metrics/postMetric";
            return requestUri;
        }

        private void OnFunctionMetricsPublishTimer(object state)
        {
            if (!string.IsNullOrEmpty(_hostRequestUri))
            {
                List<FunctionMetric> currentBatch = new List<FunctionMetric>();
                FunctionMetric currentMetric;
                while (_metrics.TryTake(out currentMetric))
                {
                    currentBatch.Add(currentMetric);
                }

                JArray payload = BuildPublishMetricPayload(currentBatch);
                var content = JsonConvert.SerializeObject(payload);
                using (var request = new HttpRequestMessage(HttpMethod.Post, _hostRequestUri))
                {
                    request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                    _httpClient.SendAsync(request);
                }
            }
        }

        private JArray BuildPublishMetricPayload(List<FunctionMetric> functionMetrics)
        {
            JArray metricsArray = new JArray();
            functionMetrics.ForEach(metric =>
            {
                metricsArray.Add(JObject.FromObject(metric));
            });
            return metricsArray;
        }
    }

    public class FunctionMetric
    {
        public DateTime TimestampUtc;

        public string MetricNamespace;

        public string MetricName;

        public long MetricValue;
    }
}
