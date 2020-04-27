using Awaitables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Xunit;

namespace Awaitables.UnitTests
{
    public class AwaitOptionTests
    {
        [Fact]
        public void Some()
        {
            var result = M();
            Assert.True(result.HasValue);
            Assert.Equal("the answer is 42", result.Value);
            static async Option<string> M()
            {
                var a = await Option.Some(42);
                var b = await Option.Some(new List<string> { "the answer is " });
                return b.First() + a;
            }
        }

        [Fact]
        public void None1()
        {
            var result = M();
            Assert.False(result.HasValue);
            Assert.Throws<InvalidOperationException>(() => result.Value);
            static async Option<string> M()
            {
                var a = await Option.None<int>();
                var b = await Option.Some(new List<string> { "the answer is " });
                return b.First() + a;
            }
        }

        [Fact]
        public void None2()
        {
            var result = M();
            Assert.False(result.HasValue);
            Assert.Throws<InvalidOperationException>(() => result.Value);
            static async Option<string> M()
            {
                var a = await Option.Some(42);
                var b = await Option.None<List<string>>();
                return b.First() + a;
            }
        }

        [Fact]
        public void NoneOnlyRunsSideEffectsBeforeNone()
        {
            var state = 0;
            var result = M();
            Assert.False(result.HasValue);
            Assert.Throws<InvalidOperationException>(() => result.Value);
            Assert.Equal(2, state);
            async Option<string> M()
            {
                state = 1;
                var a = await Option.Some(42);
                state = 2;
                var b = await Option.None<List<string>>();
                state = 3;
                return b.First() + a;
            }
        }

        [Fact]
        public void Using_WithSome()
        {
            var disposed = false;
            var result = M();
            Assert.True(result.HasValue);
            Assert.Equal("the answer is 42", result.Value);
            Assert.True(disposed);
            async Option<string> M()
            {
                using var d = new Disposable(() => disposed = true);
                var a = await Option.Some(42);
                var b = await Option.Some(new List<string> { "the answer is " });
                return b.First() + a;
            }
        }

        [Fact]
        public void Using_WithNone1()
        {
            var disposed = false;
            var result = M();
            Assert.False(result.HasValue);
            Assert.Throws<InvalidOperationException>(() => result.Value);
            Assert.True(disposed);
            async Option<string> M()
            {
                using var d = new Disposable(() => disposed = true);
                var a = await Option.None<int>();
                var b = await Option.Some(new List<string> { "the answer is " });
                return b.First() + a;
            }
        }

        [Fact]
        public void Using_WithNone2()
        {
            var disposed = false;
            var result = M();
            Assert.False(result.HasValue);
            Assert.Throws<InvalidOperationException>(() => result.Value);
            Assert.True(disposed);
            async Option<string> M()
            {
                using var d = new Disposable(() => disposed = true);
                var a = await Option.Some(42);
                var b = await Option.None<List<string>>();
                return b.First() + a;
            }
        }

        [Fact]
        public void Using_WithException1()
        {
            var disposed = false;
            Assert.Throws<Exception>(() => M());
            Assert.True(disposed);
            async Option<string> M()
            {
                using var d = new Disposable(() => disposed = true);
                if ("" == string.Empty)
                {
                    throw new Exception();
                }
                var a = await Option.Some(42);
                var b = await Option.Some(new List<string> { "the answer is " });
                return b.First() + a;
            }
        }

        [Fact]
        public void Using_WithException2()
        {
            var disposed = false;
            Assert.Throws<Exception>(() => M());
            Assert.True(disposed);
            async Option<string> M()
            {
                using var d = new Disposable(() => disposed = true);
                var a = await Option.Some(42);
                if ("" == string.Empty)
                {
                    throw new Exception();
                }
                var b = await Option.Some(new List<string> { "the answer is " });
                return b.First() + a;
            }
        }

        [Fact]
        public void Using_WithException3()
        {
            var disposed = false;
            Assert.Throws<Exception>(() => M());
            Assert.True(disposed);
            async Option<string> M()
            {
                using var d = new Disposable(() => disposed = true);
                var a = await Option.Some(42);
                var b = await Option.Some(new List<string> { "the answer is " });
                if ("" == string.Empty)
                {
                    throw new Exception();
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
