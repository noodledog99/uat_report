using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UAT_Report.Models
{
    public enum Status
    {
        NewCustomer,
        NewService,
        Renewal,
        RenewalAndChange,
        ChangeNewCustomer,
        Change,
        TerminateService,
        TerminateCustomer
    }
}
