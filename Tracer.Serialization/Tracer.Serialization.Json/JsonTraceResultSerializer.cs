using Tracer.Serialization.Abstractions;

namespace Tracer.Serialization.Json
{
    public class JsonTraceResultSerializer : ITraceResultSerializer
    {
        public string Format => throw new NotImplementedException();

        public void Serialize(TraceResult traceResult, Stream to)
        {
            throw new NotImplementedException();
        }
    }
}