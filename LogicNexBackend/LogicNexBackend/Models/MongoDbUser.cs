using MongoDB.Bson;

namespace LogicNexBackend.Models
{
    public class MongoDbuser
    {
        public ObjectId _id { get; set; }
        public int score { get; set; }
        public string UserName { get; set; }    
        public string Password_hash { get; set; }    
        public string Email { get; set; }
        public refreshTokenModel[] Refresh_tokens { get; set; }
        public bool is_active { get; set; }
        public bool Email_Confimerd { get; set; }
        public string role { get; set; }
        public string title { get; set; }
    }
}
