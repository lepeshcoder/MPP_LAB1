using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tracer.Core
{
    public class Tracer : ITracer
    {
        private ConcurrentDictionary<int, ThreadInfo> _threads = new();

        public TraceResult GetTraceResult()
        {
            Dictionary<int, ThreadInfo> traceResult = new();
            foreach (var thread in _threads)
                traceResult.Add(thread.Key, thread.Value);
            return new TraceResult(traceResult);
        }

        public void StartTrace()
        {
            int threadId = Environment.CurrentManagedThreadId;

            var threadInfo = _threads.GetOrAdd(threadId, new ThreadInfo());

            StackTrace stackTrace = new();

            var stackTraceInfo = stackTrace.GetFrame(1)!.GetMethod();
            String methodName = stackTraceInfo!.Name;
            String className = stackTraceInfo.DeclaringType!.Name;
            List<ParameterInfo> parameters = stackTraceInfo.GetParameters().ToList();
            

            Stopwatch stopwatch = new();
            var methodInfo = new MethodInfo(methodName, className, stopwatch, parameters);

            if (threadInfo.RunningMethods.Count != 0)
            {
                var parentMethod = threadInfo.RunningMethods.First();
                parentMethod.InnerMethods.Add(methodInfo);
            }
            else
            {
                threadInfo.Methods.Add(methodInfo);
            }
            threadInfo.RunningMethods.Push(methodInfo);

            stopwatch.Start();

        }

        public void StopTrace()
        {
            int threadId = Environment.CurrentManagedThreadId;

            MethodInfo? methodInfo;
            if (!_threads[threadId].RunningMethods.TryPop(out methodInfo)) return;

            methodInfo.StopWatch.Stop();
        }
    }
}
