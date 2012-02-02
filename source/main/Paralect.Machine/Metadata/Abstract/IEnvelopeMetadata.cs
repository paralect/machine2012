namespace Paralect.Machine.Metadata
{
    public interface IEnvelopeMetadata
    {
        DataType DataType { get; set; }
    }

    public enum DataType
    {
        Message,
        State
    }
}