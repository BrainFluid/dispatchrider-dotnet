using System.IO;
using Xunit;

namespace DispatchRider.AspNetCore
{
    public class DispatchRiderServiceTest
    {
        private readonly DispatchRiderService _target;

        public DispatchRiderServiceTest()
        {
            _target = new DispatchRiderService(new DispatchRiderOptions());
        }

        [Fact]
        public void ParseStream_ShouldReturnEmpty_IfNullStream()
        {
            var result = _target.ParseStream(null);
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void ParseStream_ShouldSeekToBeginningAndRead()
        {
            var s = "now is the time for all good men";
            using (var stream = new MemoryStream()) {
                var writer = new StreamWriter(stream);
                writer.Write(s);
                writer.Flush();
                Assert.NotEqual(0, stream.Position);
                var result = _target.ParseStream(stream);
                Assert.Equal(s, result);
            }
        }
    }
}