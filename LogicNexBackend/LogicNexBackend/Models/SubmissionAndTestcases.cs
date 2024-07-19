namespace LogicNexBackend.Models
{
    public class SubmissionAndTestcases
    {
        public string _id { get; set; }
        public long TestCount { get; set; }
        public string language { get; set; }
        public long Time { get; set; }
        public string Verdict { get; set; }
        public string problemname { get; set; }
        public int last_running_case { get; set; }
        public string ai_response { get; set; }
        public string submissionDate { get; set; }
    }
}
