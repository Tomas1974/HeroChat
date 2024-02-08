using System.Text.Json;
using System.Threading.Tasks;
using Fleck;
using lib;
using Service;

namespace ws;

public class ClientWantsToEnterRoomDto : BaseDto
{

    public readonly MessageService _messageService;
    
    public int roomId { get; set; }

    public ClientWantsToEnterRoomDto(MessageService messageService) 
    {
        _messageService = messageService;

    }
    
    
    public override Task Handle(ClientWantsToEnterRoomDto dto, IWebSocketConnection socket)
    {
        var message = new ServerAddsClientToRoom()
        {
            
            
            message = _messageService.GetMessages(dto.roomId)
         
        };
        //StateService.BroadcastToRoom(dto.roomId, JsonSerializer.Serialize(message));
        return Task.CompletedTask;
    }
}

public class ServerAddsClientToRoom : BaseDto
{
    public string[] message { get; set; }
    
}