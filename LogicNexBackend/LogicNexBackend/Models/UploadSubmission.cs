namespace LogicNexBackend.Models
{
    public class UploadSubmission
    {
        public string? code { get; set; }
        public string? Problem_ID { get; set; }

        public IFormFile? file { get; set; }
        public string? AI { get; set; }
        public string? tag { get; set; }

    }
}
