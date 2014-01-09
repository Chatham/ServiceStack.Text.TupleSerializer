using Xunit;

namespace ServiceStack.Text.TupleSerializer.UnitTests
{
    public class ConcurrentDictionaryCacheTests
    {
        [Fact]
        public void DefaultConstructor_DoesNotReturnNull()
        {
            var cache = new ConcurrentDictionaryCache<string, string>();
            Assert.NotNull(cache);
        }
    }
}
