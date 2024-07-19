using LogicNexBackend.Dbcontexts;
using LogicNexBackend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace LogicNexBackend.Repositories
{
    public class ProblemSetRepository
    {
        private readonly MongoDbContext _context;
        private readonly IMongoCollection<ProblemSet> _problemCollection;
        public ProblemSetRepository(MongoDbContext context)
        {
            _context = context;
            _problemCollection = _context._database.GetCollection<ProblemSet>("Problems");
        }

        public async Task<List<ProblemSet>> getProblems() {
            FilterDefinition<ProblemSet> filter = Builders<ProblemSet>
                .Filter.Empty;
            List<ProblemSet> problems = await _problemCollection.Find(filter).ToListAsync();
            return problems;

        }
        public async Task<ProblemSet> getProblems_byname(string name)
        {
            FilterDefinition<ProblemSet> filter = Builders<ProblemSet>
                .Filter.Eq(x=>x.ProblemName, name);
            ProblemSet? problem = await _problemCollection.Find(filter).FirstOrDefaultAsync();
            return problem;
        }
        public async Task<List<ProblemSet>> getProblems_byTag(string tag)
        {
            FilterDefinition<ProblemSet> filter = Builders<ProblemSet>
                .Filter.AnyEq(x=>x.Tags,tag);
            List<ProblemSet> problem = await _problemCollection.Find(filter).ToListAsync();
            return problem;
        }
    }
}
