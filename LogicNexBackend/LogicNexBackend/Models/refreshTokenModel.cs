namespace LogicNexBackend.Models
{
    public class refreshTokenModel
    {
        public string Key { get; set; }
        public string Refresh_token { get; set; }
        
        public DateTime ExpireAt { get; set; }
    }
}
