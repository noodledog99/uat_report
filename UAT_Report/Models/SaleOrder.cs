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
        [DataType(DataType.Date)]
        public DateTime SODate { get; set; }
        public string CustomerStatus { get; set; }
        public string SONumber { get; set; }
        public string Description { get; set; }
        public string OwnerService { get; set; }
        public string SubServiceName { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartingDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndingDate { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime FollowUat { get; set; }
        [DataType(DataType.Date)]
        public DateTime Receive { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
    }
}
