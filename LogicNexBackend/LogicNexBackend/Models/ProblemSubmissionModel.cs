using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicNexBackend.Models
{
    public  class ProblemSubmissionModel
    {
        public ObjectId _id { get; set; }
        public string submission_id { get; set; }
        public int time { get; set; }
        public string Langauge { get; set; }
        public int last_running_case { get; set; }
        public string UserID { get; set; }  
        public string ai_response { get; set; }  
        public string problem_name { get; set; }  
        public string status { get; set; }
        public string code { get; set; }
        public string SubmissionDate { get; set; }
    }
}
