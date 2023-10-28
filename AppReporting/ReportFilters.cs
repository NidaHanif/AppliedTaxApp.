using System.Text;

namespace AppReportClass
{
        public class ReportFilters
        {
            public string TableName { get; set; }
            public string Columns { get; set; }
            public DateTime Dt_From { get; set; }
            public DateTime Dt_To { get; set; }
            public int N_ID { get; set; }
            public int N_COA { get; set; }
            public int N_Customer { get; set; }
            public int N_Employee { get; set; }
            public int N_Project { get; set; }
            public int N_Inventory { get; set; }
            public int N_InvCategory { get; set; }
            public int N_InvSubCategory { get; set; }
            public bool All_COA { get; set; }
            public bool All_Customer { get; set; }
            public string DateFormat { get; set; }

        public string FilterText()
        {
            var Text = new StringBuilder();
            DateFormat ??= "yyyy-MM-dd";
            
            if( N_COA > 0 )
            { 
                if(Text.ToString().Length>0) { Text.Append(" AND "); }
                Text.Append($"COA={N_COA} ");
            }

            if (N_Customer > 0)
            {
                if (Text.ToString().Length > 0) { Text.Append(" AND "); }
                Text.Append($"Customer={N_Customer} ");
            }

            if (N_Employee > 0)
            {
                if (Text.ToString().Length > 0) { Text.Append(" AND "); }
                Text.Append($"Employee={N_Employee} ");
            }

            if (N_Project > 0)
            {
                if (Text.ToString().Length > 0) { Text.Append(" AND "); }
                Text.Append($"Project={N_Project} ");
            }

            if (N_Inventory > 0)
            {
                if (Text.ToString().Length > 0) { Text.Append(" AND "); }
                Text.Append($"Inventory={N_Inventory} ");
            }

            if (Dt_From > new DateTime(2022, 1, 1))
            {
                if (Text.ToString().Length > 0) { Text.Append(" AND "); }
                var _Date = Dt_From.AddDays(-1).ToString(DateFormat);
                Text.Append($"Date(Vou_Date) > Date('{_Date}')");
            }

            if (Dt_To > new DateTime(2022, 1, 1))
            {
                if (Text.ToString().Length > 0) { Text.Append(" AND "); }
                var _Date = Dt_To.AddDays(1).ToString(DateFormat);
                Text.Append($"Date(Vou_Date) < Date('{_Date}')");
            }
            return Text.ToString();
        }

    }
}
