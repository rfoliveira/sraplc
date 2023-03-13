using Microsoft.Data.Sqlite;
using Sraplc.Models;

namespace Sraplc.Repository;

public class TodoRepository : ITodoRepository
{
    private const string _dbPath = @"Data Source=.\DB\csharp.db";

    public int Create(Todo todo)
    {
        using var connection = new SqliteConnection(_dbPath);
        connection.Open();

        var insertSQL = "insert into Todo (description, completed) values (@description, @completed)";

        using var cmd = new SqliteCommand(insertSQL, connection);
        
        cmd.Parameters.AddWithValue("@description", todo.Description);
        cmd.Parameters.AddWithValue("@completed", todo.Completed);
        cmd.Prepare();
        
        return cmd.ExecuteNonQuery();
    }

    public int Delete(int id)
    {
        using var connection = new SqliteConnection(_dbPath);
        connection.Open();

        var insertSQL = "delete from Todo where id = @id";

        using var cmd = new SqliteCommand(insertSQL, connection);
        
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Prepare();
        
        return cmd.ExecuteNonQuery();
    }

    public Todo Read(int id)
    {
        using var connection = new SqliteConnection(_dbPath);
        connection.Open();

        var insertSQL = "select * from Todo where id = @id";

        using var cmd = new SqliteCommand(insertSQL, connection);
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

    public IEnumerable<Todo> ReadAll()
    {
        using var connection = new SqliteConnection(_dbPath);
        connection.Open();

        var insertSQL = "select * from Todo order by id";

        using var cmd = new SqliteCommand(insertSQL, connection);

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

    public int Update(Todo todo)
    {
        using var connection = new SqliteConnection(_dbPath);
        connection.Open();

        var insertSQL = "update Todo set description = @description, completed = @completed where id = @id";

        using var cmd = new SqliteCommand(insertSQL, connection);
        
        cmd.Parameters.AddWithValue("@description", todo.Description);
        cmd.Parameters.AddWithValue("@completed", todo.Completed);
        cmd.Parameters.AddWithValue("@id", todo.Id);
        
        return cmd.ExecuteNonQuery();
    }
}