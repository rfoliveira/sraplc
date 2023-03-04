using Microsoft.Data.Sqlite;
using Sraplc.Models;

namespace Sraplc.Repository;

public class TodoRepository : ITodoRepository
{
    private const string _dbPath = "./DB/csharp.db";

    public async Task<int> CreateAsync(Todo todo)
    {
        using var connection = new SqliteConnection(_dbPath);

        var insertSQL = "insert into Todo (description, status) values (@description, @status)";

        using var cmd = new SqliteCommand(insertSQL);
        
        cmd.Parameters.AddWithValue("@description", todo.Description);
        cmd.Parameters.AddWithValue("@status", todo.Status);
        
        await cmd.PrepareAsync();
        return await cmd.ExecuteNonQueryAsync();
    }

    public async Task<int> DeleteAsync(int id)
    {
        using var connection = new SqliteConnection(_dbPath);

        var insertSQL = "delete from Todo where id = @id";

        using var cmd = new SqliteCommand(insertSQL);
        
        cmd.Parameters.AddWithValue("@id", id);
        
        await cmd.PrepareAsync();
        return await cmd.ExecuteNonQueryAsync();
    }

    public async Task<Todo> ReadAsync(int id)
    {
        using var connection = new SqliteConnection(_dbPath);

        var insertSQL = "select * from Todo where id = @id";

        using var cmd = new SqliteCommand(insertSQL);
        
        cmd.Parameters.AddWithValue("@id", id);
        
        await cmd.PrepareAsync();
        var reader = await cmd.ExecuteReaderAsync();
        Todo todo = new();

        while (await reader.NextResultAsync())
        {
            todo.Id = reader.GetInt32(0);
            todo.Description = reader.GetString(1);
            todo.Status = reader.GetBoolean(2);
        }

        return todo;
    }

    public async Task<int> UpdateAsync(Todo todo)
    {
        using var connection = new SqliteConnection(_dbPath);

        var insertSQL = "update Todo set description = @description, status = @status where id = @id";

        using var cmd = new SqliteCommand(insertSQL);
        
        cmd.Parameters.AddWithValue("@description", todo.Description);
        cmd.Parameters.AddWithValue("@status", todo.Status);
        cmd.Parameters.AddWithValue("@id", todo.Id);
        
        await cmd.PrepareAsync();
        return await cmd.ExecuteNonQueryAsync();
    }
}