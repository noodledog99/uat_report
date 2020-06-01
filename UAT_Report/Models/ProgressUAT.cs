using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UAT_Report.Models
{
    public class ProgressUAT
    {
        public List<ProgressOnWeek> ProgressThisWeeks { get; set; }
        public List<ProgressOnWeek> ProgressLastWeeks { get; set; }
        public List<DifferenceUAT> ProgressDifferences { get; set; }
        public TotalUAT TotalProgressThisWeeks { get; set; }
        public TotalUAT TotalProgressLastWeeks { get; set; }
    }

}
