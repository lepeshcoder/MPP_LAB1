global using Xunit;

using System;
using System.Threading;

namespace Tracer.Core.Tests;

public class Test
{
    [Fact]
    public void OneThreadOneInner()
    {

        Tracer tracer = new();


        A1 a1 = new(tracer);
        a1.a();

        int id = Environment.CurrentManagedThreadId;


        var traceResult = tracer.GetTraceResult().Threads;
        Assert.Equal(traceResult.Count, 1);

        Assert.Equal(traceResult[id].Methods.Count, 1);
        Assert.Equal(traceResult[id].Methods[0].MethodClass, "A1");
        Assert.Equal(traceResult[id].Methods[0].MethodName, "a");
        Assert.InRange<long>(traceResult[id].Methods[0].StopWatch.ElapsedMilliseconds, 200, 230);

        Assert.Equal(traceResult[id].Methods[0].InnerMethods.Count, 1);
        Assert.Equal(traceResult[id].Methods[0].InnerMethods[0].MethodClass, "B1");
        Assert.Equal(traceResult[id].Methods[0].InnerMethods[0].MethodName, "b");
        Assert.InRange<long>(traceResult[id].Methods[0].InnerMethods[0].StopWatch.ElapsedMilliseconds, 100, 120);

    }


    [Fact]
    public void TwoThreadsZeroInnerMethods()
    {
        // Arrange
        Tracer tracer = new();
        A2 a = new A2(tracer);
        B2 b = new B2(tracer);

        // Act
        var t1 = new Thread(() =>
        {
            a.a();
        });

        var t2 = new Thread(() =>
        {
            b.b();
        });

        int id1 = t1.ManagedThreadId;
        int id2 = t2.ManagedThreadId;

        t1.Start();
        t2.Start();
        t1.Join();
        t2.Join();

        // Assert
        var traceResult = tracer.GetTraceResult().Threads;
        Assert.Equal(traceResult.Count, 2);

        Assert.Equal(traceResult[id1].Methods.Count, 1);
        Assert.Equal(traceResult[id1].Methods[0].MethodClass, "A2");
        Assert.Equal(traceResult[id1].Methods[0].MethodName, "a");
        Assert.InRange<long>(traceResult[id1].Methods[0].StopWatch.ElapsedMilliseconds, 100, 120);

        Assert.Equal(traceResult[id2].Methods.Count, 1);
        Assert.Equal(traceResult[id2].Methods[0].MethodClass, "B2");
        Assert.Equal(traceResult[id2].Methods[0].MethodName, "b");
        Assert.InRange<long>(traceResult[id2].Methods[0].StopWatch.ElapsedMilliseconds, 100, 120);
    }


}