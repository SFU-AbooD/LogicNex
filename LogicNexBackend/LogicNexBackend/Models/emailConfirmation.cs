using MongoDB.Bson;

namespace LogicNexBackend.Models
{
    public class emailConfirmation
    {
        public ObjectId _id { get; set; }
        public string confirmation_key { get; set; }
    }
}
