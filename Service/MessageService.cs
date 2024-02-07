using infrastructure;

namespace Service;

public class MessageService(ChatMessageService _chatMessageRepository)
{
    public IEnumerable<MessageModel> GetMessages()
    {
        return _chatMessageRepository.GetMessages();
    }
    
    public MessageModel CreateAvatar(MessageModel message)
    {
        return _chatMessageRepository.CreateMessage(message);
    }
}