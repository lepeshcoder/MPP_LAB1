using System.Xml.Serialization;
using Tracer.Core;
using Tracer.Serialization.Abstractions;

namespace Tracer.Serialization.Xml
{
    public class XmlTraceResultSerializer : ITraceResultSerializer

    {
        public string Format => "Xml";

        [Serializable]
        [XmlType(TypeName = "thread")]
        public class thread
        {

            public thread()
            {

            }
            public thread(int id, long time, List<method> methods)
            {
                this.id = id;
                this.time = $"{time}ms";
                this.methods = methods;
            }

            [XmlAttribute("id")]
            public int id { get; set; }

            [XmlAttribute("time")]
            public String time { get; set; }

            [XmlElement(ElementName = "method")]
            public List<method> methods;

        }

        [Serializable]
        [XmlType(TypeName = "method")]
        public class method
        {
            public method()
            {

            }
            public method(MethodInfo method)
            {
                
                this.name = method.MethodName;
                this.Class = method.MethodClass;
                this.time = $"{method.StopWatch.ElapsedMilliseconds}ms";
                Parameters = new();
                foreach (var param in method.Parameters)
                {
                    Parameters.Add((param.Name?.ToString()+ " " + param.ParameterType.ToString()));
                }
                if (method.InnerMethods != null)
                {
                    this.methods = new List<method>();
                    foreach (var currMethod in method.InnerMethods)
                    {
                        this.methods.Add(new method(currMethod));
                    }
                }
                else
                {
                    this.methods = null;
                }
            }

            [XmlAttribute("name")]
            public String name { get; set; }

            [XmlAttribute("class")]
            public String Class { get; set; }

            [XmlAttribute("time")]
            public String time { get; set; }

            [XmlElement(ElementName = "method")]
            public List<method>? methods;

            [XmlElement(ElementName = "params")]
            public List<string> Parameters { get; set; }
        }

        private List<thread> _ThreadResultToList(TraceResult traceResult)
        {
            List<thread> resultList = new();
            List<method> methods;
            long time;
            foreach (var thread in traceResult.Threads)
            {
                methods = new();
                time = 0;
                foreach (var method in thread.Value.Methods)
                {
                    methods.Add(new method(method));
                    time += method.StopWatch.ElapsedMilliseconds;
                }
                resultList.Add(new thread(thread.Key, time, methods));
            }
            return resultList;
        }
        public void Serialize(TraceResult traceResult, Stream to)
        {
            List<thread> serializableList = _ThreadResultToList(traceResult);
            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(List<thread>));
            xmlSerializer.Serialize(to, serializableList);
        }
    }
}