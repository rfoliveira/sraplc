using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
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
            completed INTEGER not null
        )";

        using var cmd = new SqliteCommand(createStm, conn);
        cmd.ExecuteNonQuery();

        services.AddTransient<ITodoRepository, TodoRepository>();
    }

    public static void MapTodoController(this WebApplication app)
    {
        app.MapGet("/todo", IEnumerable<Todo> (ITodoRepository todoRepo) => 
            todoRepo.ReadAll()
        )
        .WithName("Read all Todos")
        .WithOpenApi();
    
        app.MapGet("/todo/{id}", Todo (ITodoRepository todoRepo, int id) => 
            todoRepo.Read(id) 
        )
        .WithName("Read Todo by ID")
        .WithOpenApi();

        app.MapPost("/todo", IResult (ITodoRepository todoRepo, Todo todo) => 
        {
            var result = todoRepo.Create(todo);
            return TypedResults.Created("todo", result);
        })
        .Produces((int)HttpStatusCode.Created)
        .WithName("Create Todo")
        .WithOpenApi();
        
        app.MapPut("/todo", int (ITodoRepository todoRepo, Todo todo) => 
            todoRepo.Update(todo) 
        )
        .WithName("Update Todo")
        .WithOpenApi();
        
        app.MapDelete("/todo/{id}", int (ITodoRepository todoRepo, int id) => 
            todoRepo.Delete(id) 
        )
        .WithName("Delete Todo by ID")
        .WithOpenApi();
    }
}