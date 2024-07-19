using JudgeSystem.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudgeSystem.Models
{
    internal class RabbitMQmodel
    {
        public string Problem_ID { get; set; }
        public string submission_id { get; set; }

        public string code { get; set; }
        public LanguagesEnum Langauge { get; set; }
        public string UserID { get; set; }
        public string service_name { get; set; }
        public int time_limit { get; set; }

        public int memorylimit { get; set; }
    }
}
