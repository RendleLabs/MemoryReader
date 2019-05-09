using System;
using System.Text;
using SpanReaderSpike;
using Xunit;

namespace SpanReaderTests
{
    public class MemoryReaderTests
    {
        [Fact]
        public void ReadsLinesWithNewline()
        {
            var bytes = Encoding.UTF8.GetBytes("Hello\nWorld");
            var reader = MemoryReader.Create(bytes);
            Assert.Equal("Hello", reader.ReadLine());
            Assert.Equal("World", reader.ReadLine());
        }
        
        [Fact]
        public void ReadsLinesWithCarriageReturnNewline()
        {
            var bytes = Encoding.UTF8.GetBytes("Hello\r\nWorld");
            var reader = MemoryReader.Create(bytes);
            Assert.Equal("Hello", reader.ReadLine());
            Assert.Equal("World", reader.ReadLine());
        }
        
        [Fact]
        public void ReadsToEnd()
        {
            var bytes = Encoding.UTF8.GetBytes("Hello\nWorld");
            var reader = MemoryReader.Create(bytes);
            Assert.Equal("Hello\nWorld", reader.ReadToEnd());
        }
    }
}
