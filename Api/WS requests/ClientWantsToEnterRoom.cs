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
        socket.Send(JsonSerializer.Serialize(new ServerWantToEnterRoom())); //Til Test brug
        Console.WriteLine(isSuccess+" "+dto.roomId);
        
        
        return Task.CompletedTask;
    }
}


public class ServerWantToEnterRoom : BaseDto;













