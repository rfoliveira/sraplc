using Microsoft.Data.Sqlite;
using Sraplc.Repository;
using Sraplc.Service;
using Sraplc.Settings;

namespace Sraplc;

public static class Setup
{
    public static void InitDB(this IServiceCollection services, IConfiguration config)
    {
        using var conn = new SqliteConnection(config["SqliteDatabase"]);
        conn.Open();

        var createStm = @"create table if not exists Todo (
            id integer primary key autoincrement, 
            description char(200) not null, 
            completed INTEGER not null
        )";

        using var cmd = new SqliteCommand(createStm, conn);
        cmd.ExecuteNonQuery();
    }

    public static void AddServices(this IServiceCollection services, IConfiguration config)
    {
        var dbSettings = config.Get<DbSettings>() ?? new();

        services.AddSingleton<IDbSettings>(dbSettings);
        services.AddTransient<ITodoService, TodoService>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITodoRepository, TodoRepository>();
    }
} 