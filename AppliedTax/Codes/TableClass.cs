using System.Data.SQLite;
using System.Data;
using System.Reflection.Metadata;
using AppliedTax.Pages.Customer_Invoice;
using System.Text;

namespace AppliedTax.Codes
{
    public class TableClass
    {
        public SQLiteConnection MyConnection { get; set; }
        public DataTable MyDataTable { get; set; }
        public DataView MyDataView { get; set; }
        public string TableName { get; set; }

        public TableClass()
        {
            MyConnection = ConnectionClass.GetConnected();
            MyDataTable = new DataTable();
            TableName = string.Empty;
            MyDataView = new DataView();
        }

        // Constructor
        public TableClass(string TableName)
        {
            MyDataTable = new DataTable();
            
            MyConnection = ConnectionClass.GetConnected();
            string Text = $"SELECT * FROM {TableName}";
            SQLiteCommand _Command = new(Text, MyConnection);
            SQLiteDataAdapter _Adapter = new(_Command);
            DataSet _DataSet = new DataSet();
            _Adapter.Fill(_DataSet, TableName);

            if (_DataSet.Tables.Count > 0)
            {
                MyDataTable = _DataSet.Tables[0];
                MyDataView = MyDataTable.AsDataView();
                TableName = MyDataTable.TableName;
            }
        }

        public static DataRow GetRow(string TableName, int ID)
        {
            TableClass _TableClass = new(TableName);
            _TableClass.MyDataView.RowFilter = $"[ID]={ID}";
            if (_TableClass.MyDataView.Count == 1)
            {
                return _TableClass.MyDataView[0].Row;
            }

            return _TableClass.MyDataTable.NewRow();
        }

        public static DataRow GetRow(string TableName, string RecordCode)
        {
            TableClass _TableClass = new(TableName);
            _TableClass.MyDataView.RowFilter = $"[Invoice Code]='{RecordCode}'";
            if (_TableClass.MyDataView.Count > 0)
            {
                return _TableClass.MyDataView[0].Row;
            }

            return _TableClass.MyDataTable.NewRow();
        }

        public static decimal GetTaxRare(long ID)
        {
            var TaxTableClass = new TableClass("Taxes");
            TaxTableClass.MyDataView.RowFilter = $"ID={ID}";
            if(TaxTableClass.MyDataView.Count==1)
            {
                return (decimal)TaxTableClass.MyDataView[0]["Rate"];
            }
            return 0.00M;

        }



        public static string GetTitle(string TableName, long RecID) 
        {
            string _Result = string.Empty;
            string Text = $"SELECT * FROM {TableName}";
            SQLiteCommand _Command = new(Text, ConnectionClass.GetConnected());
            SQLiteDataAdapter _Adapter = new(_Command);
            DataSet _DataSet = new DataSet();
            _Adapter.Fill(_DataSet);

            if (_DataSet.Tables.Count > 0)
            {
                var MyDataTable = _DataSet.Tables[0];
                var MyDataView = MyDataTable.AsDataView();

                MyDataView.RowFilter = $"ID={RecID}";
                if(MyDataView.Count==1)
                {
                    _Result =  (string)MyDataView[0]["Title"];
                }
            }

            return _Result;
        }

        public static DataTable GetTable(string TableName)
        {
            
            string Text = $"SELECT * FROM {TableName}";
            SQLiteCommand _Command = new(Text, ConnectionClass.GetConnected());
            SQLiteDataAdapter _Adapter = new(_Command);
            DataSet _DataSet = new DataSet();
            _Adapter.Fill(_DataSet, TableName);
            if(_DataSet.Tables.Count > 0)
            {
                return _DataSet.Tables[0];
            }
            return new DataTable();

        }





        
    }

}