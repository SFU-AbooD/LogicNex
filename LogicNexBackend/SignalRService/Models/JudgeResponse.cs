using System.Runtime.CompilerServices;

namespace SignalRService.Models
{
    public class JudgeResponse
    {
        public string submissionID { get; set; }
        public string verdict { get; set; }
        public int? Current_Case { get; set; }
        public int? Time_usage { get; set; }
        public string? ai_response { get; set; }
        public int? last_running_case { get; set; }
        public bool finish { get; set; }
        public int memory_usage { get; set; }
    }
}
