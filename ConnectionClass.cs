using System.Data.SQLite;
using System.Data;

namespace AppliedTax.Codes;
    public class ConnectionClass
    {
    public static SQLiteConnection GetConnected()
    {
        var FileName = "C:\\Users\\REDHat\\OneDrive\\Desktop\\source\\source\\Tax\\AppliedTaxApp\\AppliedTax\\wwwroot\\DataBase\\AppTax.db";
        var ConnectionString = $"Data Source={FileName}";
        var connection = new SQLiteConnection(ConnectionString) ;
        connection.Open();
        return connection ;
    }
    }

