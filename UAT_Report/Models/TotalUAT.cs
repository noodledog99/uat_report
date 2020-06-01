using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UAT_Report.Models
{
    public class TotalUAT
    {
        public int TotalRequestUAT { get; set; }
        public int TotalNumberReceive { get; set; }
        public decimal TotalPercentReceive { get; set; }
        public int TotalNumberPending { get; set; }
        public decimal TotalPercentPending { get; set; }
    }
}
