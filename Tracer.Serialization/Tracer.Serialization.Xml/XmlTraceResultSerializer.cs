using Tracer.Serialization.Abstractions;

namespace Tracer.Serialization.Xml
{
    public class XmlTraceResultSerializer : ITraceResultSerializer
    {
        public string Format => throw new NotImplementedException();

        public void Serialize(TraceResult traceResult, Stream to)
        {
            throw new NotImplementedException();
        }
    }
}