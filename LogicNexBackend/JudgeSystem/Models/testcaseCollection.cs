using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudgeSystem.Models
{
    internal class testcaseCollection
    {
        public ObjectId Id { get; set; }
        public string ProblemNameCase { get; set; }
        public string Testcase { get; set; }
        public int casenumber { get; set; }
        public string answer { get; set; }
        public int timelimit { get; set; }

    }
}
