using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Awaitables
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct OptionAwaiter<T> : INotifyCompletion
    {
        private Option<T> _option;

        internal OptionAwaiter(Option<T> option)
        {
            this._option = option;
        }

        public bool IsCompleted => _option.HasValue;
        public T GetResult() => _option.Value;
        public void OnCompleted(Action completion) => throw new InvalidOperationException("Option must only be awaited in a method with a return type of Option");
    }
}