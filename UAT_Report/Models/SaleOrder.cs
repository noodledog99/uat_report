using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UAT_Report.Models
{
    public class SaleOrder
    {
        [BsonId]
        public string SOId { get; set; }

        
        public DateTime SODate { get; set; }
        
        public string Status { get; set; }
        public int SONumber { get; set; }
        public string Description { get; set; }
        public string OwnerService { get; set; }
        public string SubServiceName { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }

        
        public DateTime StartingDate { get; set; }

        
        public DateTime EndingDate { get; set; }
        
    }
}
