using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudgeSystem.Models
{
    internal class JudgeResponse
    {
        public string submissionID { get; set; }
        public string  verdict { get; set; }
        public int? Current_Case { get; set; }
        public int? last_running_case { get; set; }
        public int? Time_usage { get; set; }
        public string Ai_response { get; set; } = "";
        public bool finish { get; set; }
       public int memory_usage { get; set; }
    }
}
