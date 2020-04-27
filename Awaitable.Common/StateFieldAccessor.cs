using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Awaitables
{
    internal static class StateFieldAccessor
    {
        private delegate void RefSetterDelegate<T>(ref T t, int value);

        private static class Accessors<T> where T : IAsyncStateMachine
        {
            public static Func<T, int> _getter = CreateGetter();
            public static RefSetterDelegate<T> _setter = CreateSetter();

            private static Func<T, int> CreateGetter()
            {
                var field = typeof(T).GetField(_stateField);
                var setterMethod = new DynamicMethod(
                    name: typeof(T).FullName + ".get_state",
                    returnType: typeof(int),
                    parameterTypes: new[] { typeof(T) },
                    restrictedSkipVisibility: false);
                var gen = setterMethod.GetILGenerator();
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldfld, field);
                gen.Emit(OpCodes.Ret);
                return (Func<T, int>)setterMethod.CreateDelegate(typeof(Func<T, int>));
            }

            private static RefSetterDelegate<T> CreateSetter()
            {
                var field = typeof(T).GetField(_stateField);
                var paramType = typeof(T).IsValueType ? typeof(T).MakeByRefType() : typeof(T);
                var setterMethod = new DynamicMethod(
                    name: typeof(T).FullName + ".set_state",
                    returnType: null,
                    parameterTypes: new[] { paramType, typeof(int) },
                    restrictedSkipVisibility: false);
                var gen = setterMethod.GetILGenerator();
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Stfld, field);
                gen.Emit(OpCodes.Ret);
                return (RefSetterDelegate<T>)setterMethod.CreateDelegate(typeof(RefSetterDelegate<T>));
            }
        }

        private static string _stateField = "<>1__state";

        public static int GetState<T>(this T t) where T : IAsyncStateMachine =>
            Accessors<T>._getter(t);

        public static void SetState<T>(ref T t, int value) where T : IAsyncStateMachine =>
            Accessors<T>._setter(ref t, value);

    }
}
