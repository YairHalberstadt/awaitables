using System;

namespace Awaitables.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var exception = new AggregateException(
                new AggregateException(
                    new Exception("Bad stuff")), 
                new InvalidCastException("Bad cast!!"), 
                new AggregateException(
                    new InvalidOperationException("Bad Operation"), 
                    new AggregateException(
                        new ArgumentNullException("name"), 
                        new NullReferenceException("another null"))));

            FlattenAndLog(exception);
        }

        static async AwaitableEnumerable<Exception> FlattenAndLog(Exception exception)
        {
            while(exception is AggregateException aggregateException)
            {
                exception = await aggregateException.InnerExceptions.ToAwaitable();
                if (!(exception is AggregateException))
                {
                    Console.WriteLine(exception.Message);
                    return exception;
                }
            }
            throw new InvalidOperationException("Unreachable");
        }
    }
}
