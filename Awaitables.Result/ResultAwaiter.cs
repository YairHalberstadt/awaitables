using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Awaitables
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct ResultAwaiter<T> : INotifyCompletion, IHasException
    {
        private Result<T> _result;

        internal ResultAwaiter(Result<T> result)
        {
            this._result = result;
        }

        public bool IsCompleted => _result.IsSuccessful;
        public T GetResult() => _result.Value;
        public void OnCompleted(Action completion) => throw new InvalidOperationException("Result must only be awaited in a method with a return type of Result");
        Exception IHasException.Exception => _result.Exception;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct ResultAwaiter : INotifyCompletion, IHasException
    {
        private Result _result;

        internal ResultAwaiter(Result result)
        {
            this._result = result;
        }

        public bool IsCompleted => _result.IsSuccessful;
        public void GetResult() { }
        public void OnCompleted(Action completion) => throw new InvalidOperationException("Result must only be awaited in a method with a return type of Result");
        Exception IHasException.Exception => _result.Exception;
    }
}