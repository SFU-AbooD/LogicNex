using MongoDB.Bson;

namespace LogicNexBackend.Models
{
    public class Plan
    {
        public ObjectId _id { get; set; }
        public string planName { get; set; }
        public string email { get; set; }
        public string[] tags { get; set; }
    }
}
