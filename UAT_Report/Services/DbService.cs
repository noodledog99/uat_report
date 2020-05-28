using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UAT_Report.Models;

namespace UAT_Report.Services
{
    public class DbService : IDbService
    {
        public IMongoCollection<SaleOrder> CollectionSaleOrder { get; set; }
        public IMongoCollection<SubService> CollectionSubService { get; set; }

        public DbService(DbConfig dbConfig)
        {
            var cilent = new MongoClient(dbConfig.MongoDbConnectionString);
            var database = cilent.GetDatabase(dbConfig.MongoDbName);

            CollectionSaleOrder = database.GetCollection<SaleOrder>(dbConfig.SaleOrder);
            CollectionSubService = database.GetCollection<SubService>(dbConfig.SubService);
        }

    }
}
