using Dapper;
using Npgsql;

namespace infrastructure;

public class ChatMessageService(NpgsqlDataSource _dataSource)
{
    public string[] GetMessages (int roomId)
    {
        var sql = @"SELECT chatmessage FROM messages where Room= @roomId";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<string[]>(sql, roomId);
        }
    }
    
    public MessageModel CreateMessage(MessageModel message)
    {
        var sql =
            @" INSERT INTO messages (ChatFrom,Room,ChatMessage) VALUES (@chatFrom, @room, @chatMessage) RETURNING *;";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<MessageModel>(sql, new { chatFrom=message.ChatFrom, room=message.Room, chatMessage=message.ChatMessage});
        }
    }
}