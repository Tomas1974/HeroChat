using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text.Json;
using Fleck;
using infrastructure;
using lib;
using Service;
using System.Net.Http;
using System.Net.Http.Headers;
using a;

namespace ws;


public class ClientWantsToBroadcastToRoom : BaseEventHandler<ClientWantsToBroadcastToRoomDto>
{
    
    public readonly MessageService _messageService;
    public ClientWantsToBroadcastToRoom(MessageService messageService)
    {
        _messageService = messageService;
    }
    
    
    public override async Task Handle(ClientWantsToBroadcastToRoomDto dto, IWebSocketConnection socket)
    {
        string messageThatHasBeenChecked=await isMessageToxic(dto.message);
            
        var message = new newMessageToStore()
        {
            message = messageThatHasBeenChecked,
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
        if (messageThatHasBeenChecked != "Such speech is not allowed")
        {
            _messageService.CreateChatMessage(messageModel);
            Console.WriteLine(messageThatHasBeenChecked);
                        
        }
        }

    
    public record RequestModel(string text, List<string> categories, string outputType)
    {
        public override string ToString()
        {
            return $"{{ text = {text}, categories = {categories}, outputType = {outputType} }}";
        }
    }
    
    
    private async Task<string> isMessageToxic(string message)
    {
        
        HttpClient client = new HttpClient();
        
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://herochat.cognitiveservices.azure.com/contentsafety/text:analyze?api-version=2023-10-01");

        request.Headers.Add("accept", "application/json");
        
        //request.Headers.Add("Ocp-Apim-Subscription-Key", "025eb163a0cb4821b382cf00a2ae292c");
        request.Headers.Add("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("languageKey"));
        
        
       var req = new RequestModel(message, new List<string>() 
            { "Hate", "Violence" },
            "FourSeverityLevels");

        
        request.Content = new StringContent(JsonSerializer.Serialize(req));
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        
        HttpResponseMessage response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        var obj = JsonSerializer.Deserialize<ContentFilterResponse>(responseBody);
        var isToxic=obj.categoriesAnalysis.Count(e => e.severity > 1) >= 1;
        
        if (isToxic)
            message = "Such speech is not allowed";

        return message;
    }
    
    
}

