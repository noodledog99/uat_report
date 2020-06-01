using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UAT_Report.Models;

namespace UAT_Report.Dac
{
    public class SubServiceRepository : ISubServiceRepository
    {
        public readonly IMongoCollection<SubService> collection;
        public SubServiceRepository(DbConfig dbConfig)
        {
            var client = new MongoClient(dbConfig.MongoDbConnectionString);
            var database = client.GetDatabase(dbConfig.MongoDbName);
            collection = database.GetCollection<SubService>(dbConfig.SubService);
        }

        public void Create(SubService document)
            => collection.InsertOne(document);

        public void Create(SaleOrder model)
        {
            throw new NotImplementedException();
        }

        public SubService Get(Expression<Func<SubService, bool>> expression)
            => collection.Find(expression).FirstOrDefault();

        public IEnumerable<SubService> GetAllSubService()
            => collection.Find(it => true).ToList();

        public void Update(SubService document)
        {
            var def = Builders<SubService>.Update
               .Set(it => it.OwnerServiceName, document.OwnerServiceName)
               .Set(it => it.SubServiceName, document.SubServiceName);
            collection.UpdateOne(it => it.SubServiceId == document.SubServiceId, def);
        }



      
    }
}
