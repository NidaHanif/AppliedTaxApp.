using System.Data.SQLite;
using System.Data;
using System.Data.Entity;

namespace AppliedTax.Codes;
public class ConnectionClass
{
    public static SQLiteConnection GetConnected()
    {
        var DataBasePath = $"{Directory.GetCurrentDirectory()}\\wwwroot\\DataBase\\";
        var DataBaseFile = "AppTax.db";
        var ConnectionString = $"Data Source={DataBasePath}{DataBaseFile}";
        var connection = new SQLiteConnection(ConnectionString);
        connection.Open();
        return connection;
    }
}

