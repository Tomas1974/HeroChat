using System.Collections;
using Dapper;
using Npgsql;

namespace infrastructure;

public class ChatMessageRepository(NpgsqlDataSource _dataSource)
{
    public IEnumerable<string> GetMessages (int roomId)
    {
        var sql = @"SELECT * FROM messages where Room= @roomId";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<IEnumerable<string>>(sql, roomId);
        }
    }
    
    public MessageModel CreateMessage(MessageModel message)
    {
        var sql =
            @" INSERT INTO * (ChatFrom,Room,messages) VALUES (@chatFrom, @room, @chatMessage) RETURNING *;";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<MessageModel>(sql, new { chatFrom=message.ChatFrom, room=message.Room, chatMessage=message.ChatMessage});
        }
    }
}