using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UAT_Report.Models
{
    public class SubService
    {
        [BsonId]
        public string SubServiceId { get; set; }
        public string OwnerServiceName { get; set; }
        public string SubServiceName { get; set; }
    }
}
