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
        string[] storeMessage = new string[_messageService.GetMessages(dto.roomId).Count()];
        string[] storeFrom = new string[_messageService.GetMessages(dto.roomId).Count()];
        string[] storeRoom = new string[_messageService.GetMessages(dto.roomId).Count()];
        var i = 0;
        foreach (var item in _messageService.GetMessages(dto.roomId))
        {
            storeMessage[i] = item.ChatMessage;
            storeFrom[i] = item.ChatFrom;
            storeRoom[i] = item.RoomId+"";
            i++;
        }
        
       
        var message = new ServerSendStoredMessageToClient()
        {
            message=storeMessage,
            from = storeFrom,
            roomId = storeRoom
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
    public string[] from { get; set; }
    public string[] roomId { get; set; }
    
}