using LogicNexBackend.Dbcontexts;
using LogicNexBackend.Models;
using MongoDB.Driver;


namespace LogicNexBackend.Respositories
{
    public class ProblemSubmissionRepository
    {
        private readonly MongoDbContext _context;
        private readonly IMongoCollection<ProblemSubmissionModel> _problemSubmissionCollection;
        public ProblemSubmissionRepository(MongoDbContext context)
        {
            _context = context;
            _problemSubmissionCollection = _context._database.GetCollection<ProblemSubmissionModel>("ProblemSubmission");
        }
        public async Task createSubmission(RabbitMQmodel MQmodel) {
            ProblemSubmissionModel model = new ProblemSubmissionModel()
            {
                submission_id = MQmodel.submission_id,
                status = "in-queue",
                UserID = MQmodel.UserID,
                code = MQmodel.code,
                SubmissionDate = DateTime.UtcNow.ToString("yyyy-M-dd hh:mm:ss"),
                problem_name = MQmodel.problem_name,
                Langauge = MQmodel.language,
                last_running_case=0
            };
            await _problemSubmissionCollection.InsertOneAsync(model);
        }

        public async Task<ProblemSubmissionModel?> getSubmission(string name) { 
                FilterDefinition<ProblemSubmissionModel> filter = Builders<ProblemSubmissionModel>.Filter.Eq(x=>x.submission_id,name);
                ProblemSubmissionModel? problemSubmissionModel = await _problemSubmissionCollection.Find(filter).FirstOrDefaultAsync();
            if (problemSubmissionModel == null)
                return null;
            else
                return problemSubmissionModel;
        }
        public async Task<List<ProblemSubmissionModel>> getSubmissions(string email)
        {
            FilterDefinition<ProblemSubmissionModel> filter = Builders<ProblemSubmissionModel>.Filter.Eq(x=>x.UserID,email);
            List<ProblemSubmissionModel> problemSubmissionModel = await _problemSubmissionCollection.Find(filter).ToListAsync();
            if (problemSubmissionModel.Count == 0)
                return new();
            else
                return problemSubmissionModel;
        }
    }
}
