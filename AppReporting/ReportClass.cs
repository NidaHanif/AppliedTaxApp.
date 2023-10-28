using Microsoft.Reporting.NETCore;
using System.Data;
using System.Security.Claims;
using System.Text;

namespace AppReporting
{
    public class ReportClass
    {
        public ClaimsPrincipal AppUser { get; set; }                                // Current User Profile
        public string OutputFile { get; set; }                                            // Path + file name of printed report PDF/Doc/xls.
        public string OutputFilePath { get; set; }                                    // Path where to printed report store.
        public string OutputFileLinkPath { get; set; }                              // Location to printed report PDF.
        public string OutputFileLink { get; set; }                                      // Location to printed report PDF.

        public string ReportFilePath { get; set; }                                          // RDLC report path
        public string ReportFile { get; set; }                                                  // RDLC report path + FileName
        public string ReportFileName { get; set; }                                       // Output File .pdf, .doc or .xls
        public string RecordSort { get; set; }                                                // Sorting of recport records.
        public DataTable ReportSourceData { get; set; }                                       // DataTable to be perint in RDLC Report.
        public string ReportDataSet { get; set; }                                         // Datasource DataSet name exact in RDLC

        public Dictionary<string, string> RptParameters { get; set; } = new Dictionary<string, string>();     // Reports Paramates
        public string MyMessage { get; set; }                                          // Store message of the class
        public FileStream MyFileStream { get; set; }                                // File Stream Object
        public byte[] MyBytes { get; set; }                                               // Rendered file bytes for view or print report

        public bool IsError { get; set; }



        #region Get Reports
        public string GetReportLink()
        {
            MyBytes = GetReportbytes();

            if (MyBytes.Length > 1)
            {
                string FileName = string.Concat(OutputFilePath, OutputFile, ".pdf");
                string OutPutFile = string.Concat(OutputFileLinkPath, OutputFile, ".pdf");

                try
                {
                    if (File.Exists(FileName)) { File.Delete(FileName); }

                    using (FileStream fstream = new FileStream(FileName, FileMode.Create))
                    {
                        fstream.Write(MyBytes, 0, MyBytes.Length);
                        MyFileStream = fstream;
                    }

                    if (File.Exists(FileName)) { IsError = false; } else { IsError = true; }
                    OutputFileLink = OutPutFile;                                                                                // Supply File link if file save sucessfully.
                    MyMessage = "File has been created sucessfully.";
                    //IsError = false;
                }
                catch (Exception e) { MyMessage = e.Message; IsError = true; }

            }
            if (IsError) { OutputFileLink = ""; }
            return OutputFileLink;
        }
        public byte[] GetReportbytes()
        {
            try
            {
                var _ReportFile = string.Concat(ReportFilePath, ReportFile);
                StreamReader ReportStream = new StreamReader(_ReportFile);
                LocalReport RDLCreport = new LocalReport();
                RDLCreport.LoadReportDefinition(ReportStream);
                RDLCreport.DataSources.Add(new ReportDataSource(ReportDataSet, ReportSourceData));
                if (RptParameters.Count > 0)
                {
                    foreach (KeyValuePair<string, string> Item in RptParameters)
                    {
                        RDLCreport.SetParameters(new ReportParameter(Item.Key, Item.Value));
                    }
                }
                MyBytes = RDLCreport.Render("PDF");
            }

            catch (Exception e)
            {
                MyMessage = e.Message;
                IsError = true;
                MyBytes = new byte[] { 0 };
            }

            return MyBytes;
        }
        #endregion

        #region GetPreview(DataTable reportData)

        //private static DataTable GetPreview(DataTable reportData)
        //{
        //    // Show the Ledger if pdf fail to display ...............

        //    DataTable PreviewTable = new();
        //    PreviewTable.Columns.Add("Vou_Date");
        //    PreviewTable.Columns.Add("Vou_No");
        //    PreviewTable.Columns.Add("Description");
        //    PreviewTable.Columns.Add("DR");
        //    PreviewTable.Columns.Add("CR");
        //    PreviewTable.Columns.Add("Balance");

        //    if (reportData.Rows.Count > 0)
        //    {

        //        decimal _Balance = 0;
        //        foreach (DataRow Row in reportData.Rows)
        //        {
        //            decimal _Amount = (decimal)Row["DR"] - (decimal)Row["CR"];
        //            _Balance += _Amount;

        //            DataRow NewRow = PreviewTable.NewRow();
        //            NewRow["Vou_Date"] = Row["Vou_Date"];
        //            NewRow["Vou_No"] = Row["Vou_No"];
        //            NewRow["Description"] = Row["Description"];
        //            NewRow["DR"] = Row["DR"];
        //            NewRow["CR"] = Row["CR"];
        //            NewRow["Balance"] = _Balance;
        //            PreviewTable.Rows.Add(NewRow);
        //        }
        //    }
        //    return PreviewTable;
        //}

        #endregion

        public static string GetQueryText(AppReportClass.ReportFilters ReportFilter)
        {
            // Create a query to fatch data from Data Table for Display Reports.
            string DateFormat = "yyyy-MM-dd";
            if (ReportFilter == null) { return string.Empty; }                           // Return empty value if object is null
            string _TableName = ReportFilter.TableName.ToString();
            string _TableColumns = ReportFilter.Columns;
            StringBuilder _SQL = new();
            StringBuilder _Where = new();

            _SQL.Append("SELECT ");
            _SQL.Append(_TableColumns);
            _SQL.Append(" FROM [" + _TableName + "]");

            if (ReportFilter.Dt_From < new DateTime(2000, 1, 1)) { _Where.Append("Vou_Date>=" + ReportFilter.Dt_From.ToString(DateFormat)); }
            if (ReportFilter.Dt_To < new DateTime(2000, 1, 1)) { _Where.Append(" Vou_Date<=" + ReportFilter.Dt_To.ToString(DateFormat)); }
            if (ReportFilter.N_ID != 0) { _Where.Append(" [ID]=" + ReportFilter.N_ID.ToString() + " AND"); }
            if (ReportFilter.N_COA != 0) { _Where.Append(" [COA]=" + ReportFilter.N_COA.ToString() + " AND"); }
            if (ReportFilter.N_Customer != 0) { _Where.Append(" [Customer]=" + ReportFilter.N_Customer.ToString() + " AND"); }
            if (ReportFilter.N_Project != 0) { _Where.Append(" [Project]=" + ReportFilter.N_Project.ToString() + " AND"); }
            if (ReportFilter.N_Employee != 0) { _Where.Append(" [Employee]=" + ReportFilter.N_Employee.ToString() + " AND"); }
            if (ReportFilter.N_InvCategory != 0) { _Where.Append(" [Category]=" + ReportFilter.N_InvCategory.ToString() + " AND"); }
            if (ReportFilter.N_InvSubCategory != 0) { _Where.Append(" [SubCategory]=" + ReportFilter.N_InvSubCategory.ToString() + " AND"); }
            if (ReportFilter.N_Inventory != 0) { _Where.Append(" [Inventory]=" + ReportFilter.N_Inventory.ToString() + " AND"); }
            if (_Where.Length > 0) { _Where.Insert(0, " WHERE "); }
            string Where = _Where.ToString();
            if (Where.EndsWith("AND")) { Where = Where[..^3]; }         // Delete END Word from where

            return string.Concat(_SQL.ToString(), Where);
        }


        public class ReportData
        {
            public string ReportFile { get; set; }
            public string ReportType { get; set; }
            public string ReportExtention { get; set; }
            public string ReportMime { get; set; }
            public DataTable ReportSourceData { get; set; }
            public Dictionary<string, string> Parameters { get; set; }
        }
        //=============================================================== end.
    }


}