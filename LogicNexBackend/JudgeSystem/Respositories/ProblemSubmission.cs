using JudgeSystem.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudgeSystem.Respositories
{

    internal class ProblemSubmission
    {
        private readonly Dbcontext _context;
        public IMongoCollection<ProblemSubmissionModel> problemSubmissions { get; set; }
        public ProblemSubmission()
        {
            _context = new Dbcontext();
            problemSubmissions = _context.database.GetCollection<ProblemSubmissionModel>("ProblemSubmission");
        }
        public async Task<bool> UpdateTime(string submission_id,long time) {
            FilterDefinition<ProblemSubmissionModel> find_submission = Builders<ProblemSubmissionModel>.Filter
             .Eq(x => x.submission_id, submission_id);
            UpdateDefinition<ProblemSubmissionModel> fields_update = Builders<ProblemSubmissionModel>.Update
             .Set(x => x.time, time);
            UpdateResult result = await problemSubmissions.UpdateOneAsync(find_submission, fields_update);
            return result.ModifiedCount > 0 ? true : false;
        }
        public async Task<bool> UpdateCase(string submission_id, int last_good_case)
        {
            FilterDefinition<ProblemSubmissionModel> find_submission = Builders<ProblemSubmissionModel>.Filter
             .Eq(x => x.submission_id, submission_id);
            UpdateDefinition<ProblemSubmissionModel> fields_update = Builders<ProblemSubmissionModel>.Update
             .Set(x => x.last_running_case, last_good_case);
            UpdateResult result = await problemSubmissions.UpdateOneAsync(find_submission, fields_update);
            return result.ModifiedCount > 0 ? true : false;
        }
        public async Task<bool> UpdateSubmissionStatus(string submission_id,string status) {
            FilterDefinition<ProblemSubmissionModel> find_submission = Builders<ProblemSubmissionModel>.Filter
                .Eq(x => x.submission_id, submission_id);
            UpdateDefinition<ProblemSubmissionModel> fields_update = Builders<ProblemSubmissionModel>.Update
                .Set(x => x.status,status);
            UpdateResult result = await problemSubmissions.UpdateOneAsync(find_submission, fields_update);
            return result.ModifiedCount > 0 ? true : false;    
        }
        public async Task<bool> UpdateAiResponse(string submission_id, string ai_response)
        {
            FilterDefinition<ProblemSubmissionModel> find_submission = Builders<ProblemSubmissionModel>.Filter
                .Eq(x => x.submission_id, submission_id);
            UpdateDefinition<ProblemSubmissionModel> fields_update = Builders<ProblemSubmissionModel>.Update
                .Set(x => x.ai_response, ai_response);
            UpdateResult result = await problemSubmissions.UpdateOneAsync(find_submission, fields_update);
            return result.ModifiedCount > 0 ? true : false;
        }
    }
}
