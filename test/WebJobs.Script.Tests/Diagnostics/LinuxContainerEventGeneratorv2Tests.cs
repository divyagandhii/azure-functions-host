//// Copyright (c) .NET Foundation. All rights reserved.
//// Licensed under the MIT License. See License.txt in the project root for license information.

//using System;
//using System.Collections.Generic;
//using System.Diagnostics.Tracing;
//using System.Linq;
//using System.Text.RegularExpressions;
//using Microsoft.Azure.WebJobs.Script.WebHost.Diagnostics;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using Xunit;

//namespace Microsoft.Azure.WebJobs.Script.Tests.Diagnostics
//{
//    public class LinuxContainerEventGeneratorv2Tests
//    {
//        private readonly LinuxContainerEventGeneratorv2 _generator;
//        private readonly List<string> _events;

//        public LinuxContainerEventGeneratorv2Tests()
//        {
//            _events = new List<string>();
//            Action<string> writer = (s) =>
//            {
//                _events.Add(s);
//            };
//            _generator = new LinuxContainerEventGeneratorv2(writer);
//        }

//        [Theory]
//        [MemberData(nameof(LinuxEventGeneratorTestData.GetLogEvents), MemberType = typeof(LinuxEventGeneratorTestData))]
//        public void ParseLogEvents(LogLevel level, string subscriptionId, string appName, string functionName, string eventName, string source, string details, string summary, string exceptionType, string exceptionMessage, string functionInvocationId, string hostInstanceId, string activityId)
//        {
//            _generator.LogFunctionTraceEvent(level, subscriptionId, appName, functionName, eventName, source, details, summary, exceptionType, exceptionMessage, functionInvocationId, hostInstanceId, activityId);

//            string evt = _events.First();

//            Regex regex = new Regex(LinuxContainerEventGenerator.TraceEventRegex);
//            var match = regex.Match(evt);

//            Assert.True(match.Success);
//            Assert.Equal(19, match.Groups.Count);

//            DateTime dt;
//            var groupMatches = match.Groups.Select(p => p.Value).Skip(1).ToArray();
//            Assert.Collection(groupMatches,
//                p => Assert.Equal((int)LinuxEventGenerator.ToEventLevel(level), int.Parse(p)),
//                p => Assert.Equal(subscriptionId, p),
//                p => Assert.Equal(appName, p),
//                p => Assert.Equal(functionName, p),
//                p => Assert.Equal(eventName, p),
//                p => Assert.Equal(source, p),
//                p => Assert.Equal(details, p),
//                p => Assert.Equal(summary, p),
//                p => Assert.Equal(ScriptHost.Version, p),
//                p => Assert.True(DateTime.TryParse(p, out dt)),
//                p => Assert.Equal(exceptionType, p),
//                p => Assert.Equal(exceptionMessage, p),
//                p => Assert.Equal(functionInvocationId, p),
//                p => Assert.Equal(hostInstanceId, p),
//                p => Assert.Equal(activityId, p),
//                p => Assert.Equal(string.Empty, p),
//                p => Assert.Equal(string.Empty, p),
//                p => Assert.Equal(string.Empty, p));
//        }

//        [Theory]
//        [MemberData(nameof(LinuxEventGeneratorTestData.GetMetricEvents), MemberType = typeof(LinuxEventGeneratorTestData))]
//        public void ParseMetricEvents(string subscriptionId, string appName, string functionName, string eventName, long average, long minimum, long maximum, long count, string data)
//        {
//            _generator.LogFunctionMetricEvent(subscriptionId, appName, functionName, eventName, average, minimum, maximum, count, DateTime.Now, data);

//            string evt = _events.First();

//            Regex regex = new Regex(LinuxContainerEventGeneratorv2.MetricEventRegex);
//            var match = regex.Match(evt);

//            Assert.True(match.Success);
//            Assert.Equal(15, match.Groups.Count);

//            DateTime dt;
//            var groupMatches = match.Groups.Select(p => p.Value).Skip(1).ToArray();
//            Assert.Collection(groupMatches,
//                p => Assert.Equal(subscriptionId, p),
//                p => Assert.Equal(appName, p),
//                p => Assert.Equal(functionName, p),
//                p => Assert.Equal(eventName, p),
//                p => Assert.Equal(average, long.Parse(p)),
//                p => Assert.Equal(minimum, long.Parse(p)),
//                p => Assert.Equal(maximum, long.Parse(p)),
//                p => Assert.Equal(count, long.Parse(p)),
//                p => Assert.Equal(ScriptHost.Version, p),
//                p => Assert.True(DateTime.TryParse(p, out dt)),
//                p => Assert.Equal(data, p),
//                p => Assert.Equal(string.Empty, p),
//                p => Assert.Equal(string.Empty, p),
//                p => Assert.Equal(string.Empty, p));
//        }
//    }
//}
