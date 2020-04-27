using System;
using System.ComponentModel;

namespace Awaitables
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IHasException
    {
        internal Exception Exception { get; }
    }
}