using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UAT_Report.Models;

namespace UAT_Report.Dac
{
    public class SaleOrderRepository : ISaleOrderRepository
    {
        public readonly IMongoCollection<SaleOrder> collection;
        public SaleOrderRepository(DbConfig dbConfig)
        {
            var client = new MongoClient(dbConfig.MongoDbConnectionString);
            var database = client.GetDatabase(dbConfig.MongoDbName);
            collection = database.GetCollection<SaleOrder>(dbConfig.SaleOrder);
        }

        public void Create(SaleOrder document)
            => collection.InsertOne(document);

        public SaleOrder Get(Expression<Func<SaleOrder, bool>> expression)
            => collection.Find(expression).FirstOrDefault();

        public IEnumerable<SaleOrder> GetAllSaleOrder()
            => collection.Find(it => true).ToList();

        public void Update(SaleOrder document)
        {
            var def = Builders<SaleOrder>.Update
               .Set(it => it.Status, document.Status)
               .Set(it => it.SONumber, document.SONumber)
               .Set(it => it.Description, document.Description)
               .Set(it => it.OwnerService, document.OwnerService)
               .Set(it => it.SubServiceName, document.SubServiceName)
               .Set(it => it.CustomerNo, document.CustomerNo)
               .Set(it => it.CustomerName, document.CustomerName);
            collection.UpdateOne(it => it.SOId == document.SOId, def);
        }
    }
}
