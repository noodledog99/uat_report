using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UAT_Report
{
    public class DbConfig
    {
        public string MongoDbConnectionString { get; set; }
        public string MongoDbName { get; set; }
        public string SaleOrder { get; set; }
        public string SubService { get; set; }
    }
}
