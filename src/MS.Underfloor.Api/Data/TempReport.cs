using System;

namespace MS.Underfloor.Api.Data
{
    public class TempReport
    {
        public double[] Temps { get; set; }
        public string State { get; set; }
        public double HeaterLimitTemp { get; set; }
        public bool HeaterOn => State == "ON";
        public DateTime Timestamp { get; set; }
    }
}
