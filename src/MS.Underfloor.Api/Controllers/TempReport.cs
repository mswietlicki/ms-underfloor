using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MS.Underfloor.Api.Controllers
{
    public class TempReport {
        public double[] Temps { get; set; }
        public string State { get; set; }
    }
}
