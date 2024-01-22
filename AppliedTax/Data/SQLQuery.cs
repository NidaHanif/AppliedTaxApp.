using AppliedTax.Pages.Directory;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AppliedTax.Data
{
    public class SQLQuery
    {
        public static string GetReceivable2Submit()
        {
            var Text = new StringBuilder();
            Text.Append("SELECT ");
            Text.Append("[C].[NTN Number] AS [RegNo],");
            Text.Append("[C].[Title]      AS [Supplier Name],");
            Text.Append("'Registered'     AS [Supplier Type],");

            Text.Append("[C].[City] AS [City],");
            Text.Append("[C].[Country] AS [Country],");

            Text.Append("[T].[Title] AS [Tax Type],");
            Text.Append("[I].[Invoice Number] AS [Invoice Number],");
            Text.Append("[I].[Invoice Date] AS [Inv Date],");
            Text.Append("[I].[Payment Date] AS [Payment Date],");
            Text.Append("[P].[HSCode] AS [HSCode],");

            Text.Append("[T].[Title] AS [Sale Type],");
            Text.Append("[T].[Rate] AS [TaxRate],");
            Text.Append("[I].[Quantity] As [Quantity],");

            Text.Append("[I].[Amount] AS [Amount],");
            Text.Append("[I].[Tax Amount] AS [Tax Amount],");
            Text.Append("[I].[Net Amount] AS [Net Amount]");
            Text.Append("FROM [Invoice][I]\r\nLEFT JOIN [Company] [C] ON[C].[ID] = [I].[Company ID]\r\nLEFT JOIN [Taxes]   [T] ON[T].[ID] = [I].[TaxID]\r\nLEFT JOIN [Product] [P] ON[P].[ID] = [I].[Company ID]");
                return Text.ToString();



        }


        public static string GetPayable2Submit()
        {
            var Text = new StringBuilder();
            Text.Append("SELECT ");
            Text.Append("[C].[NTN Number] AS [RegNo],");
            Text.Append("[C].[Title]      AS [Customer Name],");
            Text.Append("'Registered'     AS [Customer Type],");

            Text.Append("[C].[City] AS [City],");
            Text.Append("[C].[Country] AS [Country],");

            Text.Append("[T].[Title] AS [Tax Type],");
            Text.Append("[Pay].[Invoice Number] AS [Number],");
            Text.Append("[Pay].[Invoice Date] AS [Inv Date],");
            Text.Append("[Pay].[Payment Date] AS [Payment Date],");
            Text.Append("[P].[HSCode] AS [HSCode],");

            Text.Append("[T].[Title] AS [Sale Type],");
            Text.Append("[T].[Rate] AS [TaxRate],");
            Text.Append("[Pay].[Quantity] AS [Quantity],");

            Text.Append("[Pay].[Amount] AS [Amount],");
            Text.Append("[Pay].[Tax Amount] AS [Tax Amount],");
            Text.Append("[Pay].[Net Amount] AS [Net Amount]");
            Text.Append("FROM [Payable] [Pay]\r\n        LEFT JOIN [Company] [C] ON[C].[ID] = [Pay].[Company ID]\r\n        LEFT JOIN [Taxes]   [T] ON[T].[ID] = [Pay].[TaxID]\r\n        LEFT JOIN [Product] [P] ON[P].[ID] = [Pay].[Company ID]");
            return Text.ToString();



        }

    }
}
