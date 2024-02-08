using infrastructure;

namespace Service;

public class MessageService(ChatMessageService _chatMessageRepository)
{
    public string[] GetMessages(int roomId)
    {
        return _chatMessageRepository.GetMessages(roomId);
    }
    
    public MessageModel CreateAvatar(MessageModel message)
    {
        return _chatMessageRepository.CreateMessage(message);
    }
}