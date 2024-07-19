using MongoDB.Bson;

namespace LogicNexBackend.Models
{
    public class Testcases
    {
        public ObjectId _id { get; set; }
        public string ProblemNameCase { get; set; }
        public int casenumber { get; set; }
        public int answer { get; set; }
        public string Testcase { get; set; }
        public int timelimit { get; set; }
    }
}
