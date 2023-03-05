using System.Net;
using Microsoft.Data.Sqlite;
using Sraplc.Models;
using Sraplc.Repository;

namespace Sraplc.Config;

public static class Setup
{
    public static void InitDB(this IServiceCollection services)
    {
        using var conn = new SqliteConnection(@"Data Source=.\DB\csharp.db");
        conn.Open();

        var createStm = @"create table if not exists Todo (
            id integer primary key autoincrement, 
            description char(200) not null, 
            status INTEGER not null
        )";

        using var cmd = new SqliteCommand(createStm, conn);
        cmd.ExecuteNonQuery();

        services.AddTransient<ITodoRepository, TodoRepository>();
    }

    public static void MapTodoController(this WebApplication app)
    {
        app.MapGet("/todo/{id}", async (ITodoRepository todoRepo, int id) => 
            await todoRepo.ReadAsync(id) 
        )
        .Produces((int)HttpStatusCode.BadRequest)
        .Produces((int)HttpStatusCode.OK)
        .WithName("Read Todo by ID")
        .WithOpenApi();

        app.MapPost("/todo", async (ITodoRepository todoRepo, Todo todo) => 
            await todoRepo.CreateAsync(todo) 
        )
        .Produces((int)HttpStatusCode.BadRequest)
        .Produces((int)HttpStatusCode.Created)
        .WithName("Create Todo")
        .WithOpenApi();
        
        app.MapPut("/todo", async (ITodoRepository todoRepo, Todo todo) => 
            await todoRepo.UpdateAsync(todo) 
        )
        .Produces((int)HttpStatusCode.NotModified)
        .Produces((int)HttpStatusCode.OK)
        .WithName("Update Todo")
        .WithOpenApi();
        
        app.MapDelete("/todo/{id}", async (ITodoRepository todoRepo, int id) => 
            await todoRepo.DeleteAsync(id) 
        )
        .Produces((int)HttpStatusCode.NotModified)
        .Produces((int)HttpStatusCode.OK)
        .WithName("Delete Todo by ID")
        .WithOpenApi();
    }
}