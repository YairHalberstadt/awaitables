using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Awaitables
{
    [AsyncMethodBuilder(typeof(OptionMethodBuilder<>))]
    public struct Option<T>
    {
        [MaybeNull] private readonly T _value;

        public Option(T value) => (_value, HasValue) = (value, true);

        public bool HasValue { get; }

        public T Value => HasValue ? _value : throw new InvalidOperationException("Option does not have a value");

        public OptionAwaiter<T> GetAwaiter() => new OptionAwaiter<T>(this);

    }

    public static class Option
    {
        public static Option<T> Some<T>(T value) => new Option<T>(value);
        public static Option<T> None<T>() => default;
    }
}
