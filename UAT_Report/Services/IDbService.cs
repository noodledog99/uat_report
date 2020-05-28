using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UAT_Report.Models;

namespace UAT_Report.Services
{
    public interface IDbService
    {
        IMongoCollection<SaleOrder> CollectionSaleOrder { get; set; }
        IMongoCollection<SubService> CollectionSubService { get; set; }
    }
}
