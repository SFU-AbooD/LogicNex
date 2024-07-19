using LogicNexBackend.CustomProcess;
using LogicNexBackend.Dbcontexts;
using LogicNexBackend.Models;
using LogicNexBackend.utils;
using Microsoft.AspNetCore.Identity.UI.Services;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace LogicNexBackend.Repositories
{

    public class PlanRepository
    {
        private readonly MongoDbContext _context;
        private readonly IMongoCollection<Plan> _PlanCollection;
        public PlanRepository(MongoDbContext context)
        {
            _context = context;
            _PlanCollection = _context._database.GetCollection<Plan>("Plans");
        }

        public async Task createPlan(Plan plan) {

            try {
                await _PlanCollection.InsertOneAsync(plan);
            }catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        public async Task<List<Plan>> getall(string email)
        {
            FilterDefinition<Plan> filter = Builders<Plan>.Filter.Eq(x => x.email, email);
            return await _PlanCollection.Find(filter).ToListAsync();
        }

        public async Task<Plan> getSpecific(string name)
        {
            FilterDefinition<Plan> filter = Builders<Plan>.Filter.Eq(x => x.planName, name);
            return await _PlanCollection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
