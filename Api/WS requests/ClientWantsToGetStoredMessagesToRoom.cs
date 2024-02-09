using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Fleck;
using infrastructure;
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
        string[] storeData = new string[_messageService.GetMessages(dto.roomId).Count()];
        var i = 0;
        foreach (var item in _messageService.GetMessages(dto.roomId))
        {
            storeData[i++] = item.ChatMessage;
            }
        
           
        
        var message = new ServerSendStoredMessageToClient()
        {
            message=storeData,
            username = StateService.Connections[socket.ConnectionInfo.Id].Username
        };
        StateService.BroadcastToRoom(dto.roomId, JsonSerializer.Serialize(
            message, new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }));
        return Task.CompletedTask;
    }
}

public class ServerSendStoredMessageToClient : BaseDto
{
    public string[] message { get; set; }
    public string username { get; set; }
}