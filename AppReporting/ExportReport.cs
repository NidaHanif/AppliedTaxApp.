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
            if (_ReportType == ReportType.PDF) { return ".pdf"; }
            if (_ReportType == ReportType.HTML) { return ".html"; }
            if (_ReportType == ReportType.Word) { return ".docx"; }
            if (_ReportType == ReportType.Excel) { return ".xlsx"; }
            return "";
        }

        public static string GetReportMime(ReportType _ReportType)
        {
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

        public byte[] Render(ReportParameters? _ReportParameter)
        {
            Variables ??= _ReportParameter;
            Variables.DataParameters ??= GetDataParameters();
            var _ReportType = _ReportParameter.ReportType;

            if (_ReportParameter.ReportType == ReportType.Preview)
            {
                _ReportType = ReportType.PDF;
            }

            var _ReportFile = string.Concat(Variables.ReportPath, Variables.ReportFile);
            ReportDataSource _DataSource = new(Variables.DataSetName, Variables.ReportData);
            LocalReport report = new();
            var _ReportStream = new StreamReader(_ReportFile);
            report.LoadReportDefinition(_ReportStream);
            report.DataSources.Add(_DataSource);
            report.SetParameters(Variables.DataParameters);
            var _RenderFormat = GetRenderFormat(_ReportType);
            Variables.FileBytes = report.Render(_RenderFormat);

            Variables.MimeType = GetReportMime(_ReportType);
            Variables.OutputFileExtention = GetReportExtention(_ReportType);
            Variables.OutputFileFullName = $"{Variables.OutputPath}{Variables.OutputFile}{Variables.OutputFileExtention}";
            Variables.OutputFileName = $"{Variables.OutputFile}{Variables.OutputFileExtention}";

            Variables.MyMessage = $"File length = {Variables.FileBytes.Length} ";
            if (Variables.ReportType == ReportType.Preview)
            {
                Variables.IsSaved = SaveReport();
            }
            return Variables.FileBytes;
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
            var _FileName = Variables.OutputFileFullName;
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
