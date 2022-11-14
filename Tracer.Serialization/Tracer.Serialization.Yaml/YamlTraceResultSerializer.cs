using Tracer.Core;
using Tracer.Serialization.Abstractions;

namespace Tracer.Serialization.Yaml
{
    public class YamlTraceResultSerializer : ITraceResultSerializer
    {
        public string Format => throw new NotImplementedException();

        public void Serialize(TraceResult traceResult, Stream to)
        {
            throw new NotImplementedException();
        }
    }
}