using Microsoft.Reporting.NETCore;
using System.Data;


namespace AppReportClass
{
    public class ReportParameters
    {
        public string ReportPath { get; set; }
        public string ReportFile { get; set; }
        public string OutputPath { get; set; }
        public string OutputPathLink { get; set; }
        public string OutputFile { get; set; }
        public string OutputFileName { get; set; }
        public string OutputFileFullName { get; set; }
        public string OutputFileExtention { get; set; }
        public string DataSetName { get; set; }
        public string MimeType { get; set; }
        public string MyMessage { get; set; }
        public string CompanyName { get; set; }
        public string Heading1 { get; set; }
        public string Heading2 { get; set; }
        public string Footer { get; set; }
        public byte[] FileBytes { get; set; }
        public bool IsSaved { get; set; } = false;

        public DataTable ReportData { get; set; }
        public ReportType ReportType { get; set; }
        public List<ReportParameter> DataParameters { get; set; }

        public string GetFileLink()
        {
            return $"{OutputPathLink}{OutputFileName}";
        }

    }
}
