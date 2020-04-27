using Awaitables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Xunit;

namespace Awaitables.UnitTests
{
    public class AwaitResultTests
    {
        [Fact]
        public void Success()
        {
            var result = M();
            Assert.True(result.IsSuccessful);
            Assert.Equal("the answer is 42", result.Value);
            static async Result<string> M()
            {
                var a = await Result.Success(42);
                var b = await Result.Success(new List<string> { "the answer is " });
                return b.First() + a;
            }
        }

        [Fact]
        public void Failure1()
        {
            var result = M();
            Assert.False(result.IsSuccessful);
            Assert.IsType<InvalidOperationException>(result.Exception);
            static async Result<string> M()
            {
                var a = await Result.Failure<int>(new InvalidOperationException());
                var b = await Result.Success(new List<string> { "the answer is " });
                return b.First() + a;
            }
        }

        [Fact]
        public void Failure2()
        {
            var result = M();
            Assert.False(result.IsSuccessful);
            Assert.IsType<InvalidOperationException>(result.Exception);
            static async Result<string> M()
            {
                var a = await Result.Success(42);
                var b = await Result.Failure<List<string>>(new InvalidOperationException());
                return b.First() + a;
            }
        }

        [Fact]
        public void FailureOnlyRunsSideEffectsBeforeFailure()
        {
            var state = 0;
            var result = M();
            Assert.False(result.IsSuccessful);
            Assert.IsType<InvalidOperationException>(result.Exception);
            Assert.Equal(2, state);
            async Result<string> M()
            {
                state = 1;
                var a = await Result.Success(42);
                state = 2;
                var b = await Result.Failure<List<string>>(new InvalidOperationException());
                state = 3;
                return b.First() + a;
            }
        }

        [Fact]
        public void Exception()
        {
            var result = M();
            Assert.False(result.IsSuccessful);
            Assert.IsType<InvalidOperationException>(result.Exception);
            static async Result<string> M()
            {
                var a = await Result.Success(42);
                if ("" == string.Empty)
                {
                    throw new InvalidOperationException();
                }
                var b = await Result.Failure<List<string>>(new InvalidOperationException());
                return b.First() + a;
            }
        }

        [Fact]
        public void ExceptionOnlyRunsSideEffectsBeforeException()
        {
            var state = 0;
            var result = M();
            Assert.False(result.IsSuccessful);
            Assert.IsType<InvalidOperationException>(result.Exception);
            Assert.Equal(2, state);
            async Result<string> M()
            {
                state = 1;
                var a = await Result.Success(42);
                state = 2;
                if ("" == string.Empty)
                {
                    throw new InvalidOperationException();
                }
                state = 3;
                var b = await Result.Failure<List<string>>(new InvalidOperationException());
                state = 4;
                return b.First() + a;
            }
        }

        [Fact]
        public void Using_WithSuccess()
        {
            var disposed = false;
            var result = M();
            Assert.True(result.IsSuccessful);
            Assert.Equal("the answer is 42", result.Value);
            Assert.True(disposed);
            async Result<string> M()
            {
                using var d = new Disposable(() => disposed = true);
                var a = await Result.Success(42);
                var b = await Result.Success(new List<string> { "the answer is " });
                return b.First() + a;
            }
        }

        [Fact]
        public void Using_WithFailure1()
        {
            var disposed = false;
            var result = M();
            Assert.False(result.IsSuccessful);
            Assert.IsType<InvalidOperationException>(result.Exception);
            Assert.True(disposed);
            async Result<string> M()
            {
                using var d = new Disposable(() => disposed = true);
                var a = await Result.Failure<int>(new InvalidOperationException());
                var b = await Result.Success(new List<string> { "the answer is " });
                return b.First() + a;
            }
        }

        [Fact]
        public void Using_WithFailure2()
        {
            var disposed = false;
            var result = M();
            Assert.False(result.IsSuccessful);
            Assert.IsType<InvalidOperationException>(result.Exception);
            Assert.True(disposed);
            async Result<string> M()
            {
                using var d = new Disposable(() => disposed = true);
                var a = await Result.Success(42);
                var b = await Result.Failure<List<string>>(new InvalidOperationException());
                return b.First() + a;
            }
        }

        [Fact]
        public void Using_WithException1()
        {
            var disposed = false;
            var result = M();
            Assert.False(result.IsSuccessful);
            Assert.IsType<InvalidOperationException>(result.Exception);
            Assert.True(disposed);
            async Result<string> M()
            {
                using var d = new Disposable(() => disposed = true);
                if ("" == string.Empty)
                {
                    throw new InvalidOperationException();
                }
                var a = await Result.Success(42);
                var b = await Result.Success(new List<string> { "the answer is " });
                return b.First() + a;
            }
        }

        [Fact]
        public void Using_WithException2()
        {
            var disposed = false;
            var result = M();
            Assert.False(result.IsSuccessful);
            Assert.IsType<InvalidOperationException>(result.Exception);
            Assert.True(disposed);
            async Result<string> M()
            {
                using var d = new Disposable(() => disposed = true);
                var a = await Result.Success(42);
                if ("" == string.Empty)
                {
                    throw new InvalidOperationException();
                }
                var b = await Result.Success(new List<string> { "the answer is " });
                return b.First() + a;
            }
        }

        [Fact]
        public void Using_WithException3()
        {
            var disposed = false;
            var result = M();
            Assert.False(result.IsSuccessful);
            Assert.IsType<InvalidOperationException>(result.Exception);
            Assert.True(disposed);
            async Result<string> M()
            {
                using var d = new Disposable(() => disposed = true);
                var a = await Result.Success(42);
                var b = await Result.Success(new List<string> { "the answer is " });
                if ("" == string.Empty)
                {
                    throw new InvalidOperationException();
                }
                return b.First() + a;
            }
        }

        private class Disposable : IDisposable
        {
            Action _action;

            public Disposable(Action action)
            {
                _action = action;
            }

            public void Dispose()
            {
                _action();
            }
        }
    }
}
