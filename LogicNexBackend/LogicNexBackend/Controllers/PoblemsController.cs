using DnsClient;
using LogicNexBackend.Models;
using LogicNexBackend.RabbitMQ;
using LogicNexBackend.Repositories;
using LogicNexBackend.Respositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Security.Claims;
using System.Text;

namespace LogicNexBackend.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [AllowAnonymous]
    public class PoblemsController : Controller
    {
        private readonly ProblemSetRepository _Problemmanager;
        private readonly ProblemSubmissionRepository _ProblemSubmissionManager;
        private readonly TestRepository _testRepository;
        private readonly ChannelCreator _channel;
        public PoblemsController(ProblemSetRepository Problemmanager,ChannelCreator channel,
             ProblemSubmissionRepository ProblemSubmissionManager,TestRepository testRepository) {
            _Problemmanager = Problemmanager;
            _channel = channel;
            _ProblemSubmissionManager = ProblemSubmissionManager;
            _testRepository = testRepository;
        }

        [HttpGet]
        [Route("problemData")]
        public async Task<IActionResult> getProblem_Details(string? submission_id) {
            if (submission_id == null) {
                return NotFound();
            }
            ProblemSubmissionModel? submission = await  _ProblemSubmissionManager.getSubmission(submission_id);
            if (submission == null) {
                return NotFound();
            }
            long number_of_test_cases = await _testRepository.getNumberOfTestcases(submission.problem_name);
            SubmissionAndTestcases verdict_result = new SubmissionAndTestcases()
            {
                language = submission.Langauge,
                problemname = submission.problem_name,
                submissionDate = submission.SubmissionDate,
                TestCount = number_of_test_cases,
                Time = submission.time,
                Verdict = submission.status,
                _id = submission._id.ToString().Substring(1, 6),
                last_running_case = submission.last_running_case,
                ai_response = submission.ai_response
                
            };
            return Json(verdict_result);
        }
        [Authorize]
        [HttpPost]
        [Route("Submission")]
        public async Task<IActionResult> submitProblem([FromForm]UploadSubmission? submission) {
            Claim? email = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            if (email == null) {
                return BadRequest();
            }
            if (submission == null || submission.Problem_ID == null|| (submission.code == null && submission.file == null))
                return BadRequest();
            if (submission.file != null)
            {
                submission.code = await new StreamReader(submission.file!.OpenReadStream()).ReadToEndAsync();
            }
            RabbitMQmodel model = new RabbitMQmodel() {
                Problem_ID = submission.Problem_ID,
                code = submission.code!,
                submission_id = Guid.NewGuid().ToString(),
                Langauge = 0,
                UserID = email.Value,
                time_limit = 2,
                memorylimit = 2000,
                problem_name = submission.Problem_ID,
                language = "c++",
                time = 0,
                AI = true,
                ai_response = "",
                tag = (submission.tag == null) ? null : submission.tag
            };

            await _ProblemSubmissionManager.createSubmission(model);
            _channel.channel.BasicPublish(
                exchange: "SubmissionExchanage",
                routingKey: "Submission",
                body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model))
                );
            return Json(new { id= model.submission_id});
        }

       [HttpGet]
        [Route("{problemName}")]
        public async Task<IActionResult> get_problem_data(string problemName) {
            ProblemSet problem = await _Problemmanager.getProblems_byname(problemName);
            if (problem == null)
                return BadRequest();

            return Json(problem);
        }
        [Authorize]
        [HttpPost]
        [Route("submissions/get")]
        public async Task<IActionResult> getsubmissions()
        {
            if (User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email) == null)
                return BadRequest();
            string email = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)!.Value;
            List<ProblemSubmissionModel> getSubmissions = await _ProblemSubmissionManager.getSubmissions(email);
            return Json(getSubmissions);
        }
        [HttpPost]
        [Route("GetProblems")]
        public async Task<IActionResult> getProblems(ProblemQuary? query)
        {

            List<ProblemSet> problems = await _Problemmanager.getProblems();
            if (query != null) {
                if (query.ProblemTypes != null && query.ProblemTypes.Length > 0) {
                        problems = problems.Where(x => query.ProblemTypes.Contains(x.Tags[0].ToLower())).ToList();
                }
                /*
                if (query.Status != null && query.Status.Length > 0) { 

                }
                */
                if (query.Ratings != null && query.Ratings.Length > 0) {
                    var easy = false;
                    var med = false;
                    var hard = false;
                    foreach(var r in query.Ratings)
                    {
                        if (r == "e") {
                            easy = true;
                        }
                        if (r == "m")
                            med = true;
                        if(r == "h")
                            hard = true;
                    }
                    if (easy == false) {
                        problems = problems.Where(x => !(x.rating >=1000 && x.rating < 1200)).ToList();
                    }
                    if (med == false)
                    {
                        problems = problems.Where(x => !(x.rating >= 1200 && x.rating <= 1400)).ToList();
                    }
                    if (hard == false)
                    {
                        problems = problems.Where(x => !(x.rating > 1400)).ToList();
                    }
                }
            }
            return Json(problems);
        }
    }
}
