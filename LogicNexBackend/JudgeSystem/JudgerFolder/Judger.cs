using JudgeSystem.CustomProcess;
using JudgeSystem.Models;
using JudgeSystem.Producer;
using JudgeSystem.Respositories;
using Newtonsoft.Json;
using MongoDB.Driver;
using System.Diagnostics;
using RabbitMQ.Client;
using System.Text;
using System.Numerics;
namespace JudgeSystem.JudgerFolder
{
    internal class Judger
    {
        private readonly ProblemSubmission _update_result;
        private readonly MessageSubmission _submission;
        private readonly IConnection _connection;
        private readonly string default_path = "F:\\submissions";
        public Judger(MessageSubmission submission, IConnection connection)
        {
            _update_result = new ProblemSubmission();
            _submission = submission;
            _connection = connection;
        }
        public async Task Judge() {
            await Task.Delay(7000); // just to make the judge system a little slower to showcase the app while navigating!
            string _fileName = Guid.NewGuid().ToString();
            string file_extention = utils.extractLanguageExtention(_submission.Langauge);
            string created_file = utils.create_file(default_path,_fileName, file_extention);
            string test_cases_file = utils.create_file(default_path,_fileName, ".txt");
            string output_file = utils.create_file(default_path,_fileName+'o', ".txt");
            await utils.Write_into_file(default_path, _fileName, file_extention, _submission.code);
            bool checkCompiledLanguage = utils.checkCompilationProcess(_submission.Langauge);
            JudgeProducer producer = new JudgeProducer(_connection, _submission.service_name);
            JudgeResponse res = new JudgeResponse();
            res.submissionID = _submission.submission_id;
            if (checkCompiledLanguage == true) {
                res.verdict = "Compilation";
                utils.PublishToSignalR(producer, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(res)));
                bool compileresult = await utils.compileCode(created_file, _submission.Langauge);
                if (compileresult == false){
                    await _update_result.UpdateSubmissionStatus(_submission.submission_id, "Compilation error");
                    // 0-Running
                    // 1-Accepted 
                    // 2-Wrong answer 
                    // 3-Compilation error
                    // 4-Runtime error
                    // 5-Time limit
                    res.verdict = "Compilation error"; // indicate this is an compilation error!
                    utils.PublishToSignalR(producer, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(res)));
                    return;
                }
              }
            res.verdict = "Running on test case 1"; // indicate the compilation was done and now its running!
            res.Current_Case = 1;
            await _update_result.UpdateSubmissionStatus(_submission.submission_id, res.verdict);
            producer._channel.BasicPublish(
            exchange: "SubmissionExchanage",
            routingKey: "SignalRQueueE",
            body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(res)));
            long time = 0;
            Testcases test_collection = new Testcases();
            var getTestCases = await test_collection.getTestCases(_submission);
                foreach (testcaseCollection cases in getTestCases) {
                await utils.Write_into_file(default_path, _fileName, ".txt",cases.Testcase);
                string get_language_runner = utils.getRunner(_submission.Langauge);
                Stopwatch watch = new Stopwatch();
                watch.Start();
                string? userOutput = await utils.runTestCase(created_file, get_language_runner,_submission.Langauge,_submission.time_limit,
                    test_cases_file,output_file,res);
                watch.Stop();
                time = Math.Max(watch.ElapsedMilliseconds,time);
                res.Time_usage = (int)time;
                string answer = cases.answer;
                answer = answer.Replace("\n\n", "\n").Replace("  ", " ");
                userOutput = userOutput.Replace("\r", "").Replace("\n\n", "\n").Replace("  ", " ");
                if (userOutput.Length > 0 && userOutput[userOutput.Length - 1] == '\n') {
                    userOutput = userOutput.Substring(0, userOutput.Length - 1);
                }
                // await _update_result.UpdateSubmissionStatus(_submission.submission_id, "Running on test case 1");
                if (userOutput == "0x44" || userOutput == "0x45" || userOutput != answer)
                {
                    if (userOutput == "0x44") {
                        res.finish = true;
                        res.verdict = $"Time limit exceeded on test case {res.Current_Case}"; // time limit
                        await _update_result.UpdateSubmissionStatus(_submission.submission_id, res.verdict);
                        utils.PublishToSignalR(producer, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(res)));
                    }
                    else if (userOutput == "0x45") {
                        res.finish = true;
                        res.verdict = $"Runtime error on test case {res.Current_Case}"; // runtime
                        await _update_result.UpdateSubmissionStatus(_submission.submission_id, res.verdict);
                        utils.PublishToSignalR(producer, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(res)));
                    }
                    else {
                        res.finish = true;
                        res.verdict = $"Wrong answer on test case {res.Current_Case}"; // wrong answer
                        await _update_result.UpdateSubmissionStatus(_submission.submission_id, res.verdict);
                        utils.PublishToSignalR(producer, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(res)));
                    }

                    break;
                }
                else {
                    if (res.Current_Case == getTestCases.Count)
                    {
                        res.finish = true;
                        res.verdict = "Accepted";
                        res.last_running_case = res.Current_Case;
                        await _update_result.UpdateSubmissionStatus(_submission.submission_id, res.verdict);
                        utils.PublishToSignalR(producer, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(res)));
                    }
                    else {
                        res.last_running_case = res.Current_Case;
                        res.Current_Case = res.Current_Case + 1;
                        res.verdict = $"Running on test case {res.Current_Case}";
                        await _update_result.UpdateSubmissionStatus(_submission.submission_id, res.verdict);
                        utils.PublishToSignalR(producer, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(res)));
                    }
                }
                await _update_result.UpdateCase(_submission.submission_id, (int)res.last_running_case);
            }
              await _update_result.UpdateTime(_submission.submission_id,time);
              File.Delete(Path.ChangeExtension(created_file,utils.getReadyExtention(_submission.Langauge)));
              File.Delete(created_file);
              File.Delete(test_cases_file);
              File.Delete(output_file);
              if (_submission.AI == true && res.verdict == "Accepted") {
                res.verdict = "AI analysis";
                utils.PublishToSignalR(producer, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(res)));
                ProcessAsync process = new ProcessAsync("cmd.exe"
              , $"/c python C:\\Users\\USER\\Documents\\ff.py",
                 input_redirect: true, output_redirect: true);
                process.Start();
                using (StreamWriter writer = process.StandardInput)
                {
                    await writer.WriteLineAsync(_submission.code);
                }
                await process.getTask();
                string output = process.StandardOutput.ReadToEnd();
                output = output.Replace("\r", "").Replace("\n\n", "\n").Replace("  ", " ");
                res.verdict = "Accepted";
                res.Ai_response = output;
                await _update_result.UpdateAiResponse(_submission.submission_id, res.Ai_response);
                utils.PublishToSignalR(producer, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(res)));
            }
            ConsumerDefinition._semaphoreSlim.Release();
          }
        }
    }

