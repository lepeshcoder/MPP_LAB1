using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracer.Core
{
    public class TraceResult
    {
        public IReadOnlyDictionary<int, > Threads { get; }

        public TraceResult(IReadOnlyDictionary<int, > threads) => Threads = threads;

    }
}
