using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace Tracer.Core
{
    public class MethodInfo
    {

        public string MethodName { get; }

        public string MethodClass { get; }

        public Stopwatch StopWatch { get; }

        // Лист со вложенными методами
        public List<MethodInfo> InnerMethods { get; } = new();

        public MethodInfo(string methodName, string methodClass, Stopwatch stopWatch)
        {
            MethodName = methodName;
            MethodClass = methodClass;
            StopWatch = stopWatch;
        }
    }
}
