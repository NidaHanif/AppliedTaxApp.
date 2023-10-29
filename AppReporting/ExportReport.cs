using Microsoft.Reporting.NETCore;

namespace AppReportClass
{
    public class ExportReport
    {
        public ReportParameters Variables { get; set; }

        #region Constructor
        public ExportReport()
        {

        }

        public ExportReport(ReportParameters parameters)
        {
            Variables = parameters;
        }
        #endregion


        #region GET Report Vales

        public static string GetRenderFormat(ReportType _ReportType)
        {
            if (_ReportType == ReportType.Preview) { return "PDF"; }
            if (_ReportType == ReportType.PDF) { return "PDF"; }
            if (_ReportType == ReportType.HTML) { return "HTML5"; }
            if (_ReportType == ReportType.Word) { return "WORDOPENXML"; }
            if (_ReportType == ReportType.Excel) { return "EXCELOPENXML"; }
            return "";
        }

        public static string GetReportExtention(ReportType _ReportType)
        {
            if (_ReportType == ReportType.Preview) { return ".pdf"; }
            if (_ReportType == ReportType.PDF) { return ".pdf"; }
            if (_ReportType == ReportType.HTML) { return ".html"; }
            if (_ReportType == ReportType.Word) { return ".docx"; }
            if (_ReportType == ReportType.Excel) { return ".xlsx"; }
            return "";
        }

        public static string GetReportMime(ReportType _ReportType)
        {
            if (_ReportType == ReportType.Preview) { return "application/pdf"; }
            if (_ReportType == ReportType.PDF) { return "application/pdf"; }
            if (_ReportType == ReportType.HTML) { return "text/html"; }
            if (_ReportType == ReportType.Word) { return "application/vnd.openxmlformats-officedocument.wordprocessingml.doc.ument"; }
            if (_ReportType == ReportType.Excel) { return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; }
            return "";
        }

        #endregion

        #region ReportExport
        public void Render()
        {
            if (Variables != null)
            {
                Render(Variables);
            }
            return;
        }

        public byte[] Render(ReportParameters _ReportParameter)
        {
            if (_ReportParameter != null)
            {
                ReportDataSource _DataSource = new(_ReportParameter.DataSetName, _ReportParameter.ReportData);
                LocalReport report = new();


                var _ReportFileName = $"{_ReportParameter.ReportPath}{_ReportParameter.ReportFile}";
                var _ReportStream = new StreamReader(_ReportFileName);
                _ReportParameter.DataParameters ??= GetDataParameters();

                report.LoadReportDefinition(_ReportStream);
                report.DataSources.Add(_DataSource);
                report.SetParameters(_ReportParameter.DataParameters);
                var _RenderFormat = GetRenderFormat(_ReportParameter.ReportType);
                _ReportParameter.FileBytes = report.Render(_RenderFormat);
                _ReportParameter.MimeType = GetReportMime(_ReportParameter.ReportType);
                _ReportParameter.OutputFileExtention = GetReportExtention(_ReportParameter.ReportType);
                _ReportParameter.OutputFileFullName = $"{_ReportParameter.OutputPath}{_ReportParameter.OutputFile}{_ReportParameter.OutputFileExtention}";
                _ReportParameter.OutputFileName = $"{_ReportParameter.OutputFile}{_ReportParameter.OutputFileExtention}";

                _ReportParameter.MyMessage = $"File length = {_ReportParameter.FileBytes.Length} ";
                if (_ReportParameter.ReportType.Equals(ReportType.Preview) || _ReportParameter.ReportType.Equals(ReportType.PDF))
                {
                    _ReportParameter.ReportType = ReportType.PDF;
                    _ReportParameter.IsSaved = SaveReport();
                }
                return _ReportParameter.FileBytes;
            }
            return Array.Empty<byte>();
        }

        private static List<ReportParameter> GetDataParameters()
        {
            // Default Parameters of each report, The following parametes will be assigned, if value is null in the report.
            List<ReportParameter> _Parameters = new List<ReportParameter>
            {
                new ReportParameter("CompanyName", "Applied Software House"),
                new ReportParameter("Heading1", "Heading "),
                new ReportParameter("Heading2", "Sub Heading"),
                new ReportParameter("Footer", "Powered by Applied Software House")
            };
            return _Parameters;
        }

        public bool SaveReport()
        {
            if (Variables.FileBytes.Length == 0)
            {
                return false;
            }

            FileStream MyFileStream;
            var _FileName = $"{Variables.OutputFileFullName}";
            if (Variables != null)
            {
                if (_FileName.Length > 0)
                {
                    if (File.Exists(_FileName)) { File.Delete(_FileName); }
                    using (FileStream fstream = new FileStream(_FileName, FileMode.Create))
                    {
                        fstream.Write(Variables.FileBytes, 0, Variables.FileBytes.Length);
                        MyFileStream = fstream;
                    }

                    if (File.Exists(_FileName))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion



    }
}
