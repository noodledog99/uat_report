using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UAT_Report.Models
{
    public class ProgressOnWeek
    {
        public string ServiceTeam { get; set; }
        public int RequestUAT { get; set; }
        public int NumberReciveUAT { get; set; }
        public int PercentReciveUAT { get; set; }
        public int NumberPendding { get; set; }
        public int PercentPendding { get; set; }
    }
}
