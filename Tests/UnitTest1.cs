using lib;
using Websocket.Client;

namespace Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
        Startup.Statup(null);
    }

    [Test]
    public void Test1()
    {
        var wsClient= new WebsocketClient(new Uri("ws://localhost:8181"));
        
    }
}