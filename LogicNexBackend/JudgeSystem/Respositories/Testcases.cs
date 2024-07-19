using JudgeSystem.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace JudgeSystem.Respositories
{
    internal class Testcases
    {
        private readonly Dbcontext _context;
        public IMongoCollection<testcaseCollection> testcases { get; set; }
        public Testcases()
        {
            _context = new Dbcontext();
            testcases = _context.database.GetCollection<testcaseCollection>("Testcases");
        }
        public async Task<List<testcaseCollection>> getTestCases(MessageSubmission _submission) {
            IMongoCollection<testcaseCollection> collection = testcases;
            FilterDefinition<testcaseCollection> items = Builders<testcaseCollection>.Filter
                .Where(x => x.ProblemNameCase == _submission.Problem_ID);
            try
            {
                List<testcaseCollection> all_test_cases = await collection.Find(items).ToListAsync();
                all_test_cases = all_test_cases.OrderBy(x => x.casenumber).ToList();
                return all_test_cases;
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return new();
        }
    }
}
