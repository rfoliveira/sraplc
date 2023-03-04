using Microsoft.Data.Sqlite;
using Sraplc.Models;
using Sraplc.Repository;

namespace Sraplc.Config;

public static class Setup
{
    public static void InitDB(this IServiceCollection services)
    {
        using var conn = new SqliteConnection("./DB/csharp.db");
        conn.Open();

        if (!TableExists(conn))
            CreateTable(conn);

        services.AddTransient<ITodoRepository, TodoRepository>();
    }

    private static void CreateTable(SqliteConnection conn)
    {
        var createStm = @"create table Todo (
            id integer primary key autoincrement, 
            description char(200) not null, 
            status INTEGER not null
        )";
        using var cmd = new SqliteCommand(createStm, conn);
        cmd.ExecuteNonQuery();
    }

    private static bool TableExists(SqliteConnection conn)
    {
        var checkStm = "select 1 from csharp where type = 'table' and name = 'Todo'";
        using var cmd = new SqliteCommand(checkStm, conn);
        
        return (bool?)cmd.ExecuteScalar() ?? false;
    }

    public static void MapTodoController(this WebApplication app)
    {
        app.MapGet("/todo/{id}", async (ITodoRepository todoRepo, int id) => 
            await todoRepo.ReadAsync(id) 
        )
        .WithName("Read Todo by ID")
        .WithOpenApi();

        app.MapPost("/todo", async (ITodoRepository todoRepo, Todo todo) => 
            await todoRepo.CreateAsync(todo) 
        )
        .WithName("Create Todo")
        .WithOpenApi();
        
        app.MapPut("/todo", async (ITodoRepository todoRepo, Todo todo) => 
            await todoRepo.UpdateAsync(todo) 
        )
        .WithName("Update Todo")
        .WithOpenApi();
        
        app.MapDelete("/todo/{id}", async (ITodoRepository todoRepo, int id) => 
            await todoRepo.DeleteAsync(id) 
        )
        .WithName("Delete Todo by ID")
        .WithOpenApi();
    }
}