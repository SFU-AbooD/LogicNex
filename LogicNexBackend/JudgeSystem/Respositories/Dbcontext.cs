using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudgeSystem.Respositories
{
    internal class Dbcontext
    {
        public IMongoClient mongoClient { get; set; }
        public IMongoDatabase database { get; set; }
        public Dbcontext()
        {
            mongoClient = new MongoClient("mongodb://localhost:27017/");
            database = mongoClient.GetDatabase("LogicNex");
        }
    }
}
