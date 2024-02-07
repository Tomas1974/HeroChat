using System.Text.Json;
using System.Threading.Tasks;
using Fleck;
using lib;
using Service;

namespace ws;

public class ClientWantsToEnterRoomDto(MessageService messageService) : BaseDto
{
    
    public override Task Handle(ClientWantsToEnterRoomDto dto, IWebSocketConnection socket)
    {
        messageService.GetMessages();
        return Task.CompletedTask;
    }
}

public class ServerAddsClientToRoom : BaseDto
{
    public string message { get; set; }
}