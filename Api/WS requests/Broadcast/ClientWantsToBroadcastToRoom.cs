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
        /**************** Sprog censur udføres ****************************************/
        string messageThatHasBeenChecked=await isMessageToxic(dto.message);
        
        /**************** Sender message til frontend modtagere ***********************/
        var message = new newMessageToStore()
        {
            message = messageThatHasBeenChecked,
            from = StateService.Connections[socket.ConnectionInfo.Id].Username,
            roomId = dto.roomId+""
            
        };
        
        StateService.BroadcastToRoom(dto.roomId, JsonSerializer.Serialize(
            message));
       
        
        /**************** Gemmer data i messagemodel til gemning i database ************/
        MessageModel messageModel= new MessageModel()
        {
            ChatMessage = dto.message,
            ChatFrom = StateService.Connections[socket.ConnectionInfo.Id].Username,
            RoomId = dto.roomId,
                    
        };
        /**************** Gemmer i database **********************************************/
        if (messageThatHasBeenChecked != "Such speech is not allowed")
        {
            _messageService.CreateChatMessage(messageModel);
            Console.WriteLine(messageThatHasBeenChecked);
                        
        }
      }

    
    private async Task<string> isMessageToxic(string message)
    {
        HttpRequestMessage request = produceHttpMessage(message);
        ContentFilterResponse obj= await getContentFilterResponse(request);
        
        
        var isToxic=obj.categoriesAnalysis.Count(e => e.severity > 1) >= 1;
        
        if (isToxic)
            message = "Such speech is not allowed";

        return message;
    }

   

    private HttpRequestMessage produceHttpMessage(string message)
    {
       
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://herochat.cognitiveservices.azure.com/contentsafety/text:analyze?api-version=2023-10-01");

        request.Headers.Add("accept", "application/json");
        request.Headers.Add("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("languageKey"));
        
        var messageToCensorship = new RequestModel
        {
            text = message,
            categories = new List<string>() { "Hate", "Violence" },
            outputType = "FourSeverityLevels"
        };

        request.Content = new StringContent(JsonSerializer.Serialize(messageToCensorship));
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        return request;
    }
    
    
    private async Task<ContentFilterResponse> getContentFilterResponse(HttpRequestMessage request)
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        ContentFilterResponse obj = JsonSerializer.Deserialize<ContentFilterResponse>(responseBody);
        
        return obj;
    }
    
    
}


