using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicNexBackend.Models
{
    public class RabbitMQmodel
    {
        public string Problem_ID { get; set; }
        public string submission_id { get; set; }
        public int time { get; set; }
        public string problem_name { get; set; }
        public bool AI { get; set; }
        public string language { get; set; }
        public string ai_response { get; set; }
        public string tag { get; set; }
        public string code { get; set; }
        public int Langauge { get; set; }
        public string UserID { get; set; }
        public string service_name { get; set; }
        public int time_limit { get; set; }

        public int memorylimit { get; set; }
    }
}
