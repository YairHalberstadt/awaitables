using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Awaitables
{
    internal static class StateMachineCopier
    {
        private static class MemberwiseCloneDelegateStore<T>
        {
            public static readonly Func<object, object> _clone = CreateClone();

            private static Func<object, object> CreateClone()
            {
                var method = typeof(T).GetMethod(nameof(MemberwiseClone), BindingFlags.NonPublic | BindingFlags.Instance);
                return (Func<object, object>)method.CreateDelegate(typeof(Func<object, object>));
            }
        }


        public static T Copy<T>(this T stateMachine) where T : IAsyncStateMachine
        {
            if (default(T) is null)
            {
                return (T)MemberwiseCloneDelegateStore<T>._clone(stateMachine);
            }
            else
            {
                return stateMachine;
            }
        }
    }
}
