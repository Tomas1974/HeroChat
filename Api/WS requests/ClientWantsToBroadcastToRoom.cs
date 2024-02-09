using System.Text.Encodings.Web;
using System.Text.Json;
using Fleck;
using infrastructure;
using lib;
using Service;

namespace ws;

public class ClientWantsToBroadcastToRoomDto : BaseDto
{
    public string message { get; set; }
    public int roomId { get; set; }
}

public class ClientWantsToBroadcastToRoom : BaseEventHandler<ClientWantsToBroadcastToRoomDto>
{
    
    public readonly MessageService _messageService;
    public ClientWantsToBroadcastToRoom(MessageService messageService)
    {
        _messageService = messageService;
    }
    public override Task Handle(ClientWantsToBroadcastToRoomDto dto, IWebSocketConnection socket)
    {
            MessageModel messageModel= new MessageModel()
                {
                    ChatMessage = dto.message,
                    ChatFrom = StateService.Connections[socket.ConnectionInfo.Id].Username,
                    RoomId = dto.roomId,
                    
                };
            
            _messageService.CreateChatMessage(messageModel);
            

        
            
        var message = new newMessageToStore()
        {
            message = dto.message,
            username = StateService.Connections[socket.ConnectionInfo.Id].Username,
            roomId = dto.roomId
            
        };
        
        StateService.BroadcastToRoom(dto.roomId, JsonSerializer.Serialize(
            message));
        
        return Task.CompletedTask;
    }
}

public class newMessageToStore : BaseDto
{
    public string message { get; set; }
    public string username { get; set; }
    
    public int roomId { get; set; }
}