namespace Paralect.Machine.Messages
{
    public interface IPacketMessageEnvelope : IMessageEnvelope
    {
        IPacketMessageEnvelope CloneEnvelope();

        byte[] MetadataBinary { get; }
        byte[] MessageBinary { get; }
    }
}