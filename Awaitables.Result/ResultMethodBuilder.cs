using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace Awaitables
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct ResultMethodBuilder<T>
    {
        public static ResultMethodBuilder<T> Create() => new ResultMethodBuilder<T>();

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine => stateMachine.MoveNext();

        public void SetStateMachine(IAsyncStateMachine stateMachine) { }
        public void SetException(Exception exception) => Task = Result.Failure<T>(exception);
        public void SetResult(T result) => Task = Result.Success<T>(result);

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion, IHasException
            where TStateMachine : IAsyncStateMachine
        {
            Task = Result.Failure<T>(awaiter.Exception);
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            throw new InvalidOperationException("An async method returning a Result can only await a Result");
        }

        public Result<T> Task { get; private set; }
    }
}