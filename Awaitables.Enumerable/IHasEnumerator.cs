using System.Collections;
using System.ComponentModel;

namespace Awaitables
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IHasEnumerator
    {
        internal IEnumerator Enumerator { get; }
    }
}