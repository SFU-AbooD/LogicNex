using MongoDB.Bson;

namespace LogicNexBackend.Models
{
    public class ProblemSet
    {
        public ObjectId _id { get; set; }
        public string[] Tags { get; set; }
        public string ProblemName { get; set; }
        public int rating { get; set; }
        public string showtest { get; set; }
        public string showoutput { get; set; }
        public int? contest_id { get; set; }
        public int? timelimit { get; set; }
        public string? problemStatement { get; set; }
    }
}
