using System;
using System.Buffers;
using System.Text;
using System.Xml.Linq;
using SpanReaderSpike;
using Xunit;

namespace SpanReaderTests
{
    public class MemoryReaderAsXmlSourceTests
    {
        [Fact]
        public void CanParseSingleLineXml()
        {
            using (var reader = MemoryReader.Create(Encoding.UTF8.GetBytes(SingleLine)))
            {
                var xml = XElement.Load(reader);
                Assert.Equal("root", xml.Name);
            }
        }
        
        [Fact]
        public void CanParseMultiLineXml()
        {
            using (var reader = MemoryReader.Create(Encoding.UTF8.GetBytes(MultiLine)))
            {
                var xml = XElement.Load(reader);
                Assert.Equal("root", xml.Name);
            }
        }
        
        [Fact]
        public void CanParseMultiLineXmlWithEmptyLines()
        {
            using (var reader = MemoryReader.Create(Encoding.UTF8.GetBytes(MultiLineWithEmptyLines)))
            {
                var xml = XElement.Load(reader);
                Assert.Equal("root", xml.Name);
            }
        }
        
        [Fact]
        public void CanParseMultiLineXmlWithEmptyLinesFromSequence()
        {
            var bytes = Encoding.UTF8.GetBytes(MultiLineWithEmptyLines);

            var segment1 = new TestSegment(bytes.AsMemory(0, 10), 0);
            var segment2 = new TestSegment(bytes.AsMemory(10, 10), 10);
            var segment3 = new TestSegment(bytes.AsMemory(20), 20);
            
            segment1.SetNext(segment2);
            segment2.SetNext(segment3);

            var sequence = new ReadOnlySequence<byte>(segment1, 0, segment3, segment3.Memory.Length);
            
            using (var reader = MemoryReader.Create(sequence))
            {
                var xml = XElement.Load(reader);
                Assert.Equal("root", xml.Name);
            }
        }
        
        private const string SingleLine = "<root><element>Foo</element><element>Bar</element></root>";
        private const string MultiLine = @"<root>
  <element>Foo</element>
  <element>Bar</element>
</root>";
        private const string MultiLineWithEmptyLines = @"
<root>

  <element>Foo</element>
  
  <element>Bar</element>
  
</root>

";
    }

    public class TestSegment : ReadOnlySequenceSegment<byte>
    {
        public TestSegment(ReadOnlyMemory<byte> memory, long runningIndex)
        {
            Memory = memory;
            RunningIndex = runningIndex;
        }

        public void SetNext(TestSegment next)
        {
            Next = next;
        }
    }
}