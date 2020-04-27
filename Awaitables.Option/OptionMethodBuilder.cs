using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
namespace Awaitables
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct OptionMethodBuilder<T>
    {
        public static OptionMethodBuilder<T> Create() => new OptionMethodBuilder<T>();

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine => stateMachine.MoveNext();

        public void SetStateMachine(IAsyncStateMachine stateMachine) { }
        public void SetException(Exception exception) => ExceptionDispatchInfo.Capture(exception).Throw();
        public void SetResult(T result) => Task = new Option<T>(result);

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
        }

        public Option<T> Task { get; private set; }
    }
}