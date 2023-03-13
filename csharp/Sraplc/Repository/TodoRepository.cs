using Microsoft.Data.Sqlite;
using Sraplc.Models;
using Sraplc.Settings;

namespace Sraplc.Repository;

public class TodoRepository : ITodoRepository
{
    private readonly SqliteConnection _conn;

    public TodoRepository(IDbSettings dbSettings)
    {
        _conn = new SqliteConnection(dbSettings.SqliteDatabase);
    }

    private void OpenConnection()
    {
        if (_conn.State == System.Data.ConnectionState.Closed)
            _conn.Open();
    }

    public int Create(Todo todo)
    {
        OpenConnection();

        var sql = "insert into Todo (description, completed) values (@description, @completed)";

        using var cmd = new SqliteCommand(sql, _conn);
        
        cmd.Parameters.AddWithValue("@description", todo.Description);
        cmd.Parameters.AddWithValue("@completed", todo.Completed);
        cmd.Prepare();
        
        return cmd.ExecuteNonQuery();
    }

    public int Delete(int id)
    {
        OpenConnection();

        var sql = "delete from Todo where id = @id";

        using var cmd = new SqliteCommand(sql, _conn);
        
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Prepare();
        
        return cmd.ExecuteNonQuery();
    }

    public IEnumerable<Todo> GetAll()
    {      
        OpenConnection();
        
        var sql = "select * from Todo order by id";

        using var cmd = new SqliteCommand(sql, _conn);

        var reader = cmd.ExecuteReader();
        var todos = new List<Todo>();

        while (reader.Read())
        {
            todos.Add(new Todo {
                Id = reader.GetInt32(0),
                Description = reader.GetString(1),
                Completed = reader.GetBoolean(2)
            });
        }

        return todos;
    }

    public Todo GetBy(int id)
    {
        OpenConnection();

        var sql = "select * from Todo where id = @id";

        using var cmd = new SqliteCommand(sql, _conn);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Prepare();

        var reader = cmd.ExecuteReader();
        Todo todo = new();

        while (reader.Read())
        {
            todo.Id = reader.GetInt32(0);
            todo.Description = reader.GetString(1);
            todo.Completed = reader.GetBoolean(2);
        }

        return todo;
    }

    public int Update(Todo todo)
    {
        OpenConnection();

        var sql = "update Todo set description = @description, completed = @completed where id = @id";

        using var cmd = new SqliteCommand(sql, _conn);
        
        cmd.Parameters.AddWithValue("@description", todo.Description);
        cmd.Parameters.AddWithValue("@completed", todo.Completed);
        cmd.Parameters.AddWithValue("@id", todo.Id);
        
        return cmd.ExecuteNonQuery();
    }
}