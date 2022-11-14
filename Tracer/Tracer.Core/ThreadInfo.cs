using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracer.Core
{
    public class ThreadInfo
    {
        public List<MethodInfo> Methods = new();
        public Stack<MethodInfo> RunningMethods = new();
    }
}
