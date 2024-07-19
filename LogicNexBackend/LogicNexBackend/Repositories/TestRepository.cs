using LogicNexBackend.Dbcontexts;
using LogicNexBackend.Models;
using LogicNexBackend.utils;
using Microsoft.AspNetCore.Identity.UI.Services;
using MongoDB.Driver;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace LogicNexBackend.Repositories
{

    public class TestRepository
    {
        private readonly MongoDbContext _context;
        private readonly IMongoCollection<Testcases> _TestCollection;
        public TestRepository(MongoDbContext context)
        {
            _context = context;
            _TestCollection = _context._database.GetCollection<Testcases>("Testcases");
        }

        public async Task<long> getNumberOfTestcases(string problem_name)
        {
            FilterDefinition<Testcases> filter = Builders<Testcases>.
                Filter.Eq(x=>x.ProblemNameCase,problem_name);
            long count = await _TestCollection.CountDocumentsAsync(filter);
            return count;
        }

    }
}
