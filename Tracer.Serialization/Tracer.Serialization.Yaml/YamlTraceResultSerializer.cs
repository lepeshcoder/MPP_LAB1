using System.Text;
using Tracer.Core;
using Tracer.Serialization.Abstractions;
using YamlDotNet.Serialization;

namespace Tracer.Serialization.Yaml
{
    public class YamlTraceResultSerializer : ITraceResultSerializer
    {
        public string Format => "Yaml";

        private class _ThreadInfo
        {

            public _ThreadInfo(int id, long time, List<_MethodInfo> methods)
            {
                this.id = id;
                this.time = $"{time}ms";
                this.methods = methods;
            }

            public int id;

            public String time;

            public List<_MethodInfo> methods;

        }

        private class _MethodInfo
        {

            public _MethodInfo(MethodInfo method)
            {

                this.name = method.MethodName;
                this.Class = method.MethodClass;
                this.time = $"{method.StopWatch.ElapsedMilliseconds}ms";
                if (method.InnerMethods != null)
                {
                    this.methods = new List<_MethodInfo>();
                    foreach (var currMethod in method.InnerMethods)
                    {
                        this.methods.Add(new _MethodInfo(currMethod));
                    }
                }
                else
                {
                    this.methods = null;
                }
            }
            public String name;

            public String Class;

            public String time;

            public List<_MethodInfo>? methods;
        }

        private List<_ThreadInfo> _ThreadResultToList(TraceResult traceResult)
        {
            List<_ThreadInfo> resultList = new();
            List<_MethodInfo> methods;
            long time;
            foreach (var thread in traceResult.Threads)
            {
                methods = new();
                time = 0;
                foreach (var method in thread.Value.Methods)
                {
                    methods.Add(new _MethodInfo(method));
                    time += method.StopWatch.ElapsedMilliseconds;
                }
                resultList.Add(new _ThreadInfo(thread.Key, time, methods));
            }
            return resultList;
        }
        public void Serialize(TraceResult traceResult, Stream to)
        {
            List<_ThreadInfo> serializableList = _ThreadResultToList(traceResult);

            var serializer = new SerializerBuilder().DisableAliases().Build();
            var result = serializer.Serialize(serializableList);
            to.Write(Encoding.UTF8.GetBytes(result));
        }

    }
}