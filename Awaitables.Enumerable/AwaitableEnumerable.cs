using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Awaitables
{
    [AsyncMethodBuilder(typeof(EnumerableMethodBuilder<>))]
    public struct AwaitableEnumerable<T> : IEnumerable<T>
    {
        public AwaitableEnumerable(IEnumerable<T> underlying)
        {
            _underlying = underlying;
        }

        public IEnumerable<T> _underlying { get; }

        public IEnumerator<T> GetEnumerator()
        {
            return _underlying.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _underlying.GetEnumerator();
        }

        public EnumerableAwaiter<T> GetAwaiter() => new EnumerableAwaiter<T>(this);
    }

    public static class AwaitableEnumerable
    {
        public static AwaitableEnumerable<T> ToAwaitable<T>(this IEnumerable<T> enumerable) => new AwaitableEnumerable<T>(enumerable);
    }
}
