using System.Collections;
using Dapper;
using Npgsql;

namespace infrastructure;

public class ChatMessageRepository(NpgsqlDataSource _dataSource)
{
    public IEnumerable<ResponseModel> GetMessages (int roomId)
    {
        var sql = @"SELECT chatmessage FROM messages where roomid= @roomId";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<ResponseModel>(sql, roomId);
        }
    }
    
    public MessageModel CreateMessage(MessageModel message)
    {
        var sql =
            @" INSERT INTO messages (ChatFrom,roomid,chatmessage) VALUES (@chatFrom, @roomId, @chatMessage) RETURNING *;";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<MessageModel>(sql, new { chatFrom=message.ChatFrom, roomId=message.RoomId, chatMessage=message.ChatMessage});
        }
    }
}