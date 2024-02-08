using System.Collections;
using infrastructure;

namespace Service;

public class MessageService(ChatMessageRepository _chatMessageRepository)
{
    
    
    public IEnumerable<string> GetMessages(int roomId)
    {
        return _chatMessageRepository.GetMessages(roomId);
    }
    
    public MessageModel CreateAvatar(MessageModel message)
    {
        return _chatMessageRepository.CreateMessage(message);
    }
}