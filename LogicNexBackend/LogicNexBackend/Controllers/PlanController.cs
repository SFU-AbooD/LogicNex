using LogicNexBackend.CustomProcess;
using LogicNexBackend.Models;
using LogicNexBackend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;

namespace LogicNexBackend.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class PlanController : Controller
    {
        private readonly PlanRepository _planRepository;
        private readonly ProblemSetRepository _problemSetRepository;
        public PlanController(PlanRepository planRepository, ProblemSetRepository _repo)
        {
            _planRepository = planRepository;
            _problemSetRepository = _repo;
        }
        [HttpPost]
        [Route("create")]
        [Authorize]
        public async Task<IActionResult> create(PlanRequest plan)
        {
            ProcessAsync process = new ProcessAsync("cmd.exe"
        , "/c python C:\\Users\\USER\\PycharmProjects\\ai_learn\\file.py 1",
        input_redirect: true, output_redirect: true);
            process.Start();
            using (StreamWriter writer = process.StandardInput)
            {
                await writer.WriteLineAsync(plan.explain);
            }
            await process.getTask();
            string tags = await process.StandardOutput.ReadToEndAsync();
            tags = tags.Replace("\n", "").Replace("\b", "").Replace("\r", "");
            string tagsarr = tags.Split("plan=")[1];
            string[] tag_ = tagsarr.Split(",");
            Plan plan_model = new Plan()
            {
                email = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)!.Value,
                tags = tag_,
                planName = plan.name
            };
            await _planRepository.createPlan(plan_model);
            return Ok();
        }
        [HttpGet]
        [Route("get")]
        [Authorize]
        public async Task<List<Plan>> get()
        {
            string email = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)!.Value;
            List<Plan> plans = await _planRepository.getall(email);
            return plans;
        }
        [HttpGet]
        [Route("problems")]
        [Authorize]
        public async Task<List<List<ProblemSet>>> problems(string? name)
        {
            List<List<ProblemSet>> solve = new();
            string[] tags = (await _planRepository.getSpecific(name)).tags;
            foreach (string tag in tags) { 
                List<ProblemSet> problemSet = await _problemSetRepository.getProblems_byTag(tag);
                solve.Add(problemSet);
            }
            return solve;
        }
    }
}
