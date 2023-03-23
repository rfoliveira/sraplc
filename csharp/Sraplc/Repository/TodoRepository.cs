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
            _conn.OpenAsync();
    }

    public async Task<int> CreateAsync(Todo todo)
    {
        OpenConnection();

        var sql = "insert into Todo (description, completed) values (@description, @completed)";

        using var cmd = new SqliteCommand(sql, _conn);
        
        cmd.Parameters.AddWithValue("@description", todo.Description);
        cmd.Parameters.AddWithValue("@completed", todo.Completed);
        await cmd.PrepareAsync();
        
        return await cmd.ExecuteNonQueryAsync();
    }

    public async Task<int> DeleteAsync(int id)
    {
        OpenConnection();

        var sql = "delete from Todo where id = @id";

        using var cmd = new SqliteCommand(sql, _conn);
        
        cmd.Parameters.AddWithValue("@id", id);
        await cmd.PrepareAsync();
        
        return await cmd.ExecuteNonQueryAsync();
    }

    public async Task<IEnumerable<Todo>> GetAllAsync()
    {      
        OpenConnection();
        
        var sql = "select * from Todo order by id";

        using var cmd = new SqliteCommand(sql, _conn);

        var reader = await cmd.ExecuteReaderAsync();
        var todos = new List<Todo>();

        while (await reader.ReadAsync())
        {
            todos.Add(new Todo {
                Id = reader.GetInt32(0),
                Description = reader.GetString(1),
                Completed = reader.GetBoolean(2)
            });
        }

        return await Task.FromResult(todos);
    }

    public async Task<Todo> GetByAsync(int id)
    {
        OpenConnection();

        var sql = "select * from Todo where id = @id";

        using var cmd = new SqliteCommand(sql, _conn);
        cmd.Parameters.AddWithValue("@id", id);
        await cmd.PrepareAsync();

        var reader = await cmd.ExecuteReaderAsync();
        Todo todo = new();

        while (await reader.ReadAsync())
        {
            todo.Id = reader.GetInt32(0);
            todo.Description = reader.GetString(1);
            todo.Completed = reader.GetBoolean(2);
        }

        return await Task.FromResult(todo);
    }

    public async Task<int> UpdateAsync(Todo todo)
    {
        OpenConnection();

        var sql = "update Todo set description = @description, completed = @completed where id = @id";

        using var cmd = new SqliteCommand(sql, _conn);
        
        cmd.Parameters.AddWithValue("@description", todo.Description);
        cmd.Parameters.AddWithValue("@completed", todo.Completed);
        cmd.Parameters.AddWithValue("@id", todo.Id);
        await cmd.PrepareAsync();
        
        return await cmd.ExecuteNonQueryAsync();
    }
}