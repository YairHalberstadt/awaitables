using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Awaitables
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct ResultAwaiter<T> : INotifyCompletion, IHasException
    {
        private Result<T> _result;

        internal ResultAwaiter(Result<T> option)
        {
            this._result = option;
        }

        public bool IsCompleted => _result.IsSuccessful;
        public T GetResult() => _result.Value;
        public void OnCompleted(Action completion) => throw new InvalidOperationException("Result must only be awaited in a method with a return type of Result");
        Exception IHasException.Exception => _result.Exception;
    }
}