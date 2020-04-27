using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Awaitables
{
    public struct EnumerableAwaiter<T> : INotifyCompletion, IHasEnumerator
    {
        private readonly IEnumerator<T> _enumerator;

        public EnumerableAwaiter(AwaitableEnumerable<T> awaitableEnumerable)
        {
            _enumerator = awaitableEnumerable.GetEnumerator();
        }

        public bool IsCompleted => false;

        IEnumerator IHasEnumerator.Enumerator => _enumerator;

        public T GetResult() => _enumerator.Current;

        public void OnCompleted(Action continuation) => throw new InvalidOperationException("AwaitableEnumerable must only be awaited in a method with a return type of AwaitableEnumerable");
    }
}