using System.Text.Json;
using System.Threading.Tasks;
using Fleck;
using lib;
using Service;
using System.Text.Json;
using Fleck;
using lib;
using ws;


public class ClientWantsToEnterRoomDto : BaseDto
{
    public int roomId { get; set; }
}

public class ClientWantsToEnterRoom : BaseEventHandler<ClientWantsToEnterRoomDto>
{

    public readonly MessageService _messageService;
    
    public ClientWantsToEnterRoom(MessageService messageService)
    {
        _messageService = messageService;

    }
    
    public override Task Handle(ClientWantsToEnterRoomDto dto, IWebSocketConnection socket)
    {
        var isSuccess = StateService.AddToRoom(socket, dto.roomId);
        Console.WriteLine(isSuccess+" "+dto.roomId);
        
        socket.Send(JsonSerializer.Serialize(new ServerAddsClientToRoom()
        {
            message = "you were succesfully added to room with ID " + dto.roomId
        }));
        return Task.CompletedTask;
    }
}

public class ServerAddsClientToRoom : BaseDto
{
    public string message { get; set; }
}














