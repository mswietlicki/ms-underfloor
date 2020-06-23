using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace MS.Underfloor.Api.Data
{
    internal class TempReportEntity : TableEntity
    {
        public TempReportEntity(DateTime timestamp)
        {
            PartitionKey = timestamp.Date.ToString("ddMMyyyy");
            RowKey = timestamp.ToString("HHmmss");
            Timestamp = timestamp;
        }

        public TempReportEntity()
        {
        }

        public string Temps { get; set; }
        public double HeaterLimitTemp { get; set; }
        public bool HeaterOn { get; set; }
    }
}
