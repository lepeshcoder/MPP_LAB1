﻿using Tracer.Core;
using Tracer.Serialization;

internal class Program
{
    private static void Main(string[] args)
    {
        Tracer.Core.Tracer tracer = new();
        Foo foo = new(tracer);
        Pup pup = new(tracer);

        var t1 = new Thread(() =>
        {
            foo.MyMethod();
        });

        var t2 = new Thread(() =>
        {
            pup.PupMethod(5,"lepesh");
        });

        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();

        var traceResult = tracer.GetTraceResult();

        Serializers.SerializeIntoFiles(traceResult, "Plugins", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Results"));
    }
}

public class Foo
{
    private Bar _bar;
    
    private Pup _pup;
    
    private ITracer _tracer;

    internal Foo(ITracer tracer)
    {
        _tracer = tracer;
        _bar = new Bar(_tracer);
        _pup = new Pup(_tracer);
    }

    public void MyMethod()
    {
        _tracer.StartTrace();
        Thread.Sleep(50);
        _bar.InnerMethod();
        _pup.PupMethod(5,"lepesh");
        _tracer.StopTrace();
    }
}

public class Bar
{
    private ITracer _tracer;

    internal Bar(ITracer tracer)
    {
        _tracer = tracer;
    }

    public void InnerMethod()
    {
        _tracer.StartTrace();
        Thread.Sleep(100);
        _tracer.StopTrace();
    }
}

public class Pup
{
    private ITracer _tracer;

    internal Pup(ITracer tracer)
    {
        _tracer = tracer;
    }

    public void PupMethod(int a, string b)
    {
        _tracer.StartTrace();
        Thread.Sleep(50);
        _tracer.StopTrace();
    }
}