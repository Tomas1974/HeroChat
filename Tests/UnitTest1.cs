using Dapper;
using lib;
using Npgsql;
using Websocket.Client;
using ws;

namespace Tests;

public class Tests
{
    private static readonly Uri Uri = new(Environment.GetEnvironmentVariable("pgconn")!);
    

    [SetUp]
    public void Setup()
    {
        Startup.Statup(null);
    }

    [Test]
    public async Task Test1()
    {
        var ws = await new WebSocketTestClient().ConnectAsync();
        var ws2 = await new WebSocketTestClient().ConnectAsync();
        await ws.DoAndAssert(new ClientWantsToSignInDto()
        {
            Username = "Bob"
        }, r => r.Count(dto => dto.eventType == nameof(ServerWelcomesUser)) == 1);
        await ws2.DoAndAssert(new ClientWantsToSignInDto()
        {
            Username = "Alice"
        },r => r.Count(dto => dto.eventType == nameof(ServerWelcomesUser)) == 1);


        await ws.DoAndAssert(new ClientWantsToEnterRoomDto()
        {
            roomId = 1
        },r => r.Count(dto => dto.eventType == nameof(ServerWantToEnterRoom)) == 1);
        await ws2.DoAndAssert(new ClientWantsToEnterRoomDto()
        {
            roomId = 1
        },r => r.Count(dto => dto.eventType == nameof(ServerWantToEnterRoom)) == 1);

         await ws.DoAndAssert(new ClientWantsToBroadcastToRoomDto()
         {
            message = "ABCDEFGHIJKLMNOPQRSTUVXYZÆØÅ",
            roomId = 1
            
         },r => r.Count(dto => dto.eventType == nameof(newMessageToStore)) == 1);
         await ws2.DoAndAssert(new ClientWantsToBroadcastToRoomDto()
         {
             roomId = 1,
             message = "ABCDEFGHIJKLMNOPQRSTUVXYZÆØÅ"
         },r => r.Count(dto => dto.eventType == nameof(newMessageToStore)) == 2);
         
         
         
         await ws.DoAndAssert(new ClientWantsToGetStoredMessagesToRoomDto()
         {
             roomId = 1
         },r => r.Count(dto => dto.eventType == nameof(ServerSendStoredMessageToClient)) == 1);
      
         
         await ws2.DoAndAssert(new ClientWantsToGetStoredMessagesToRoomDto()
         {
             roomId = 1
         },r => r.Count(dto => dto.eventType == nameof(ServerSendStoredMessageToClient)) == 1);

         using (var conn = Helper.DataSource.OpenConnection())
         {
             conn.Execute("DELETE FROM messages WHERE chatmessage = 'ABCDEFGHIJKLMNOPQRSTUVXYZÆØÅ'");
         }
    }
}