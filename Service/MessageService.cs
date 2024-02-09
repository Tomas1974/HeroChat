using System.Collections;
using infrastructure;

namespace Service;

public class MessageService(ChatMessageRepository _chatMessageRepository)
{
    
    
    public IEnumerable<ResponseModel> GetMessages(int roomId)
    {
        return _chatMessageRepository.GetMessages(roomId);
    }
    
    public MessageModel CreateChatMessage(MessageModel message)
    {
        return _chatMessageRepository.CreateMessage(message);
    }
}