
namespace Paralect.Machine.Messages
{
    /// <summary>
    /// Why Command Receiver and not Command Recipient?
    /// 
    /// "A recipient is generally a person who receives gifts and communications.
    ///  A receiver can be the same, but it's usually an electronic device that receives signals (such as a radio or a telephone headset), 
    ///  or in American football it is the player who is good at being in the right place to catch the ball that is thrown. 
    ///  A receiver can also be a person who receives stolen goods, or he can be the person whom the court appoints to manage 
    ///  the financial affairs of someone who is bankrupt."
    ///  http://www.english-test.net/forum/ftopic25013.html
    /// </summary>
    public interface ICommandMetadata : IMessageMetadata
    {
        
    }
}