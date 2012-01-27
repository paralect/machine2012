namespace Paralect.Machine.Messages
{
    public class BinaryMessageEnvelope
    {
        public byte[] Header { get; set; }
        public byte[] Message { get; set; }
    }
}