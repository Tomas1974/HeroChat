using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text.Json;
using Fleck;
using infrastructure;
using lib;
using Service;
using System.Net.Http;
using System.Net.Http.Headers;

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
    public override async Task Handle(ClientWantsToBroadcastToRoomDto dto, IWebSocketConnection socket)
    {
        await this.isMessageToxic(dto.message);
            

        
            
        var message = new newMessageToStore()
        {
            message = dto.message,
            from = StateService.Connections[socket.ConnectionInfo.Id].Username,
            roomId = dto.roomId+""
            
        };
        
        StateService.BroadcastToRoom(dto.roomId, JsonSerializer.Serialize(
            message));
       
        
        MessageModel messageModel= new MessageModel()
        {
            ChatMessage = dto.message,
            ChatFrom = StateService.Connections[socket.ConnectionInfo.Id].Username,
            RoomId = dto.roomId,
                    
        };
            
          _messageService.CreateChatMessage(messageModel);
        
     
    }

    private async Task isMessageToxic(string message)
    {
        

        HttpClient client = new HttpClient();

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://herochat.cognitiveservices.azure.com/contentsafety/text:analyze?api-version=2023-10-01");

        request.Headers.Add("accept", "application/json");
        request.Headers.Add("Ocp-Apim-Subscription-Key", "025eb163a0cb4821b382cf00a2ae292c");


        var req = new RequestModel
        {
            text = message,
            categories = new List<string>() { "Hate", "Violence" },
            outputType = "FourSeverityLevels"
        };
        
        request.Content = new StringContent(JsonSerializer.Serialize(req));
    //    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json", Environment.GetEnvironmentVariable("KEY"));
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
    ;

        
        HttpResponseMessage response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        var obj = JsonSerializer.Deserialize<ContentFilterResponse>(responseBody);
        var isToxic=obj.categoriesAnalysis.Count(e => e.severity > 1) >= 1;
        if (isToxic)
            throw new ValidationException("Such speech is not allowed");
    }
    
    
    
    
    
}

public class RequestModel 
{
    public string text { get; set; }
    public List<string> categories { get; set; }
    public string outputType { get; set; }
     
}

public class newMessageToStore : BaseDto
{
    public string message { get; set; }
    public string from { get; set; }
    
    public string roomId { get; set; }
}




public class CategoriesAnalysis
{
    public string category { get; set; }
    public int severity { get; set; }
}

public class ContentFilterResponse
{
    public List<object> blocklistsMatch { get; set; }
    public List<CategoriesAnalysis> categoriesAnalysis { get; set; }
}

