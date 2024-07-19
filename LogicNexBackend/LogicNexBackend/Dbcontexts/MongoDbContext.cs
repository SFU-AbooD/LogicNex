using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LogicNexBackend.Dbcontexts
{
    public class MongoDbContext
    {
        private readonly MongoClientSettings _options;
        private readonly MongoClient _client;
        public readonly IMongoDatabase _database;
        public MongoDbContext(IOptions<MongoClientSettings> options)
        {
            //_options = options.Value;
            _client = new MongoClient("mongodb://localhost:27017/");
            _database = _client.GetDatabase("LogicNex");
        }
    }
}
