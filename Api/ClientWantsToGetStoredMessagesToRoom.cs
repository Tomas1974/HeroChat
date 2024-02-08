using System.Text.Json;
using System.Threading.Tasks;
using Fleck;
using lib;
using Service;

namespace ws;

public class ClientWantsToGetStoredMessagesToRoomDto : BaseDto
{
   
    public int roomId { get; set; }
}



public class ClientWantsToGetStoredMessagesToRoom : BaseEventHandler<ClientWantsToGetStoredMessagesToRoomDto>
{
    public readonly MessageService _messageService;
    
    public ClientWantsToGetStoredMessagesToRoom(MessageService messageService)
    {
        _messageService = messageService;
    }
    
    
    public override Task Handle(ClientWantsToGetStoredMessagesToRoomDto dto, IWebSocketConnection socket)
    {
        var message = new ServerSendStoredMessageToClient()
        {
            message=_messageService.GetMessages(dto.roomId),
            username = StateService.Connections[socket.ConnectionInfo.Id].Username
        };
        StateService.BroadcastToRoom(dto.roomId, JsonSerializer.Serialize(
            message));
        return Task.CompletedTask;
    }
}

public class ServerSendStoredMessageToClient : BaseDto
{
    public IEnumerable<string> message { get; set; }
    public string username { get; set; }
}