using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
namespace Awaitables
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct EnumerableMethodBuilder<T>
    {
        private List<T> _resultsBuilder;
        public static EnumerableMethodBuilder<T> Create()
        {
            var builder = new List<T>();
            return new EnumerableMethodBuilder<T>
            {
                _resultsBuilder = builder,
                Task = new AwaitableEnumerable<T>(builder),
            };
        }

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine => stateMachine.MoveNext();

        public void SetStateMachine(IAsyncStateMachine stateMachine) { }
        public void SetException(Exception exception) => ExceptionDispatchInfo.Capture(exception).Throw();
        public void SetResult(T result) => _resultsBuilder.Add(result);

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion, IHasEnumerator
            where TStateMachine : IAsyncStateMachine
        {
            var enumerator = awaiter.Enumerator;
            while(enumerator.MoveNext())
            {
                var copy = stateMachine.Copy();
                copy.MoveNext();
            }
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            throw new InvalidOperationException("An async method returning an AwaitableEnumerable can only await a AwaitableEnumerable");
        }

        public AwaitableEnumerable<T> Task { get; private set; }
    }
}