using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UAT_Report.Models
{
    public class DifferenceUAT
    {
        public string ServiceTeam { get; set; }
        public int RequestUAT { get; set; }
        public int ReceiveUAT { get; set; }
    }
}
