using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Awaitables
{
    [AsyncMethodBuilder(typeof(ResultMethodBuilder<>))]
    public struct Result<T>
    {
        [MaybeNull] private readonly T _value;
        private readonly Exception? _exception;

        public Result(T value) => (_value, _exception) = (value, null);
        public Result(Exception exception) => (_value, _exception) = (default, exception ?? throw new ArgumentNullException(nameof(exception)));

        public Exception Exception => _exception ?? throw new InvalidOperationException("Result is successful");

        public T Value => _exception is null ? _value : throw new InvalidOperationException("Result is failed");

        public ResultAwaiter<T> GetAwaiter() => new ResultAwaiter<T>(this);

        public bool IsSuccessful => _exception is null;

        public bool IsFailed => !IsSuccessful;
    }

    public static class Result
    {
        public static Result<T> Success<T>(T value) => new Result<T>(value);
        public static Result<T> Failure<T>(Exception exception) => new Result<T>(exception);
    }
}
