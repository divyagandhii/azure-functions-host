//// Copyright (c) .NET Foundation. All rights reserved.
//// Licensed under the MIT License. See License.txt in the project root for license information.

//using System;
//using Microsoft.Extensions.Logging;

//namespace Microsoft.Azure.WebJobs.Script.WebHost.Diagnostics
//{
//    internal class LinuxContainerEventGeneratorv2 : LinuxContainerEventGenerator
//    {
//        private readonly string _containerName;
//        private readonly string _stampName;
//        private readonly string _tenantId;

//        public LinuxContainerEventGeneratorv2(Action<string> writeEvent = null) : base(writeEvent)
//        {
//            _stampName = Environment.GetEnvironmentVariable("WEBSITE_CURRENT_STAMPNAME")?.ToLowerInvariant();
//            _tenantId = Environment.GetEnvironmentVariable("WEBSITE_STAMP_DEPLOYMENT_ID")?.ToLowerInvariant();
//            _containerName = Environment.GetEnvironmentVariable("CONTAINER_NAME")?.ToLowerInvariant();
//        }

//        public static string TraceEventv2Regex { get; } = $"{ScriptConstants.LinuxLogEventStreamName} (?<Level>[0-6]),(?<SubscriptionId>[^,]*),(?<AppName>[^,]*),(?<FunctionName>[^,]*),(?<EventName>[^,]*),(?<Source>[^,]*),\"(?<Details>.*)\",\"(?<Summary>.*)\",(?<HostVersion>[^,]*),(?<EventTimestamp>[^,]+),(?<ExceptionType>[^,]*),\"(?<ExceptionMessage>.*)\",(?<FunctionInvocationId>[^,]*),(?<HostInstanceId>[^,]*),(?<ActivityId>[^,\"]*),\"(?<ContainerName>[^,]*)\",(?<StampName>[^,]*),(?<TenantId>[^,]*)";

//        public static string MetricEventv2Regex { get; } = $"{ScriptConstants.LinuxMetricEventStreamName} (?<SubscriptionId>[^,]*),(?<AppName>[^,]*),(?<FunctionName>[^,]*),(?<EventName>[^,]*),(?<Average>\\d*),(?<Min>\\d*),(?<Max>\\d*),(?<Count>\\d*),(?<HostVersion>[^,]*),(?<EventTimestamp>[^,]+),\"(?<Data>.*)\",(?<ContainerName>[^,]*),(?<StampName>[^,]*),(?<TenantId>[^,]*)";

//        public override void LogFunctionTraceEvent(LogLevel level, string subscriptionId, string appName, string functionName, string eventName, string source, string details, string summary, string exceptionType, string exceptionMessage, string functionInvocationId, string hostInstanceId, string activityId)
//        {
//            string eventTimestamp = DateTime.UtcNow.ToString(EventTimestampFormat);
//            string hostVersion = ScriptHost.Version;
//            FunctionsSystemLogsEventSource.Instance.SetActivityId(activityId);
//            string testMessage = "dummyMessage";

//            // A schema change here requires corresponding server side change.
//            WriteEvent($"{ScriptConstants.LinuxLogEventStreamName} {(int)ToEventLevel(level)},{subscriptionId},{appName},{functionName},{eventName},{source},{NormalizeString(details)},{summary},{hostVersion},{eventTimestamp},{exceptionType},{NormalizeString(exceptionMessage)},{functionInvocationId},{hostInstanceId},{activityId},{_containerName},{_stampName},{_tenantId}");
//            WriteEvent($"{ScriptConstants.LinuxLogEventStreamName} {(int)ToEventLevel(level)}, {testMessage}");
//           // base.LogFunctionTraceEvent(level, subscriptionId, appName, functionName, eventName, source, details, summary, exceptionType, exceptionMessage, functionInvocationId, hostInstanceId, activityId);
//        }

//        public override void LogFunctionMetricEvent(string subscriptionId, string appName, string functionName, string eventName, long average, long minimum, long maximum, long count, DateTime eventTimestamp, string data)
//        {
//            string hostVersion = ScriptHost.Version;

//            // A schema change here requires corresponding server side change.
//            WriteEvent($"{ScriptConstants.LinuxMetricEventStreamName} {subscriptionId},{appName},{functionName},{eventName},{average},{minimum},{maximum},{count},{hostVersion},{eventTimestamp.ToString(EventTimestampFormat)},{NormalizeString(data)},{_containerName},{_stampName},{_tenantId},,,,");
//          //  base.LogFunctionMetricEvent(subscriptionId, appName, functionName, eventName, average, minimum, maximum, count, eventTimestamp, data);
//        }
//    }
//}