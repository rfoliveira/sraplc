namespace Sraplc.Settings;

public class DbSettings : IDbSettings
{
    public string SqliteDatabase { get; set; } = default!;
}