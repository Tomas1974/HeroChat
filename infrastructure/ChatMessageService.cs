using Dapper;
using Npgsql;

namespace infrastructure;

public class ChatMessageService(NpgsqlDataSource _dataSource)
{
    public IEnumerable<MessageModel> GetMessages ()
    {
        var sql = @"SELECT * FROM webshop.avatar where deleted=true ORDER BY avatar_id;";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<MessageModel>(sql);
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