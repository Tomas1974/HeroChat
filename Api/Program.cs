using System;
using System.Reflection;
using Dapper;
using Fleck;
using infrastructure;
using lib;
using Microsoft.AspNetCore.Builder;
using Npgsql;
using Service;
using ws;

public static class Startup
{
   

public static void Main(string[] args)
{
    Statup(args);
    Console.ReadLine();
}

public static void Statup(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);

    if (builder.Environment.IsDevelopment())
    {
        builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString,
            dataSourceBuilder => dataSourceBuilder.EnableParameterLogging());
    }

    if (builder.Environment.IsProduction())
    {
        builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString);
    }

    builder.Services.AddSingleton<ChatMessageRepository>();
    builder.Services.AddSingleton<MessageService>();

    var clientEventHandlers = builder.FindAndInjectClientEventHandlers(Assembly.GetExecutingAssembly());

    var app = builder.Build();
    
    var server = new WebSocketServer("ws://0.0.0.0:8181");


    server.Start(ws =>
    {
        ws.OnOpen = () =>
        {
            StateService.AddConnection(ws);
        };
        ws.OnMessage = async message =>
        {
            // evaluate whether or not message.eventType == 
            // trigger event handler
            try
            {
                await app.InvokeClientEventHandler(clientEventHandlers, ws, message);

            }
            catch (Exception e)
            {
                ws.Send(e.Message);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
                Console.WriteLine(e.StackTrace);
                // your exception handling here
            }
        };
    });
}
}



