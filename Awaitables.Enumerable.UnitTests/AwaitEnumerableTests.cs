using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Xunit;

namespace Awaitables.UnitTests
{
    public class AwaitEnumerableTests
    {
        [Fact]
        public void AwaitMany()
        {
            var result = M();
            Assert.Equal(new[] { "Lorem1", "Ipsum1", "dolor1", "sit1", "Lorem2", "Ipsum2", "dolor2", "sit2" }, result);
            async AwaitableEnumerable<string> M()
            {
                var a = await new[] { 1, 2 }.ToAwaitable();
                var b = await new[] { new List<string> { "Lorem", "Ipsum", "dolor" }, new List<string> { "sit" }, new List<string> { } }.ToAwaitable();
                return await b.ToAwaitable() + a;
            }
        }

        [Fact]
        public void SideEffectsCalledOncePerItemsInPrecedingEnumerable()
        {
            var countFirst = 0;
            var countSecond = 0;
            var countThird = 0;
            var countFourth = 0;
            var result = M();
            Assert.Equal(new[] { "Lorem1", "Ipsum1", "dolor1", "sit1", "Lorem2", "Ipsum2", "dolor2", "sit2" }, result);
            Assert.Equal(1, countFirst);
            Assert.Equal(2, countSecond);
            Assert.Equal(6, countThird);
            Assert.Equal(8, countFourth);

            async AwaitableEnumerable<string> M()
            {
                countFirst++;
                var a = await new[] { 1, 2 }.ToAwaitable();
                countSecond++;
                var b = await new[] { new List<string> { "Lorem", "Ipsum", "dolor" }, new List<string> { "sit" }, new List<string> { } }.ToAwaitable();
                countThird++;
                var c = await b.ToAwaitable();
                countFourth++;
                return c + a;
            }
        }

        [Fact]
        public void SideEffectsNeverCalledIfEnumerableIsEmpty()
        {
            var countFirst = 0;
            var countSecond = 0;
            var countThird = 0;
            var countFourth = 0;
            var result = M();
            Assert.Equal(Array.Empty<string>(), result);
            Assert.Equal(1, countFirst);
            Assert.Equal(2, countSecond);
            Assert.Equal(0, countThird);
            Assert.Equal(0, countFourth);

            async AwaitableEnumerable<string> M()
            {
                countFirst++;
                var a = await new[] { 1, 2 }.ToAwaitable();
                countSecond++;
                var b = await Array.Empty<List<string>>().ToAwaitable();
                countThird++;
                var c = await b.ToAwaitable();
                countFourth++;
                return c + a;
            }
        }
    }
}
