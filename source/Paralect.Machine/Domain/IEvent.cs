namespace Paralect.Machine.Domain
{
    /// <summary>
    /// Domain Event
    /// </summary>
    public partial interface IEvent
    {
        EventMetadata Metadata { get; set; }
    }
}