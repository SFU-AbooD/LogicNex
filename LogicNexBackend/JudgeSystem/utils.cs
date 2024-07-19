using JudgeSystem.CustomProcess;
using JudgeSystem.Languages;
using JudgeSystem.Models;
using JudgeSystem.Producer;
using RabbitMQ.Client;

namespace JudgeSystem
{
    internal static class utils
    {
        public static void PublishToSignalR(JudgeProducer producer, byte[]body) {
            producer._channel.BasicPublish(
                exchange: "SubmissionExchanage", 
                routingKey: "SignalRQueueE",
                body: body);
        }
        public static string create_file(string path,string file_name,string extention) {
            string File_location = Path.ChangeExtension(Path.Combine(path, file_name),extention);
            using FileStream fileStream = File.Create(File_location);
            return File_location;
        }
        public static async Task<string> Write_into_file(string path, string file_name, string extention,string code)
        {
            string File_location = Path.ChangeExtension(Path.Combine(path, file_name), extention);
            using StreamWriter create_file = new StreamWriter(File_location); // using will close this after the end of the current block
            await create_file.WriteLineAsync(code);
            return File_location;
        }
        public static async Task<string> ReadFileAsync(string path)
        {
            using StreamReader sr = new StreamReader(path);
            return await sr.ReadToEndAsync();
        }
        public static async Task<bool> compileCode(string _fileName,LanguagesEnum language) {
            ProcessAsync compilation;
            string _fileNamecc = Path.ChangeExtension(_fileName, getReadyExtention(language));
            switch (language)
            {
                case LanguagesEnum.cpp17:
                    compilation = new ProcessAsync("g++", $"{_fileName} -w -std=c++17 -o {_fileNamecc}");
                    break;
                case LanguagesEnum.cpp14:
                    compilation = new ProcessAsync("g++", $"{_fileName} -w -std=c++14 -o {_fileNamecc}");
                    break;
                default:
                    compilation = new ProcessAsync("g++", $"{_fileName} -w -std=c++17 -o {_fileNamecc}");
                    break;
            }
            compilation.Start();
            await compilation.getTask();
            if (compilation.ExitCode != 0)
            {
                return false;
            }
            else{
                return true;
            }
        }
        public static string getRunner(LanguagesEnum languages) {
            switch (languages) {
                case LanguagesEnum.cpp17:
                case LanguagesEnum.cpp14:
                    return "g++";
                default:
                    return "g++";
            }
        }
        public static async Task<string> runTestCase(string file_name,string runner, LanguagesEnum languages,int timelimit
            ,string test_cases_file,string _output_path, JudgeResponse res) {
            string File_location = Path.ChangeExtension(file_name, getReadyExtention(languages));
            using CustomCancellationSource ct = new CustomCancellationSource();
            ProcessAsync Runner = new ProcessAsync("cmd.exe"
                , $"/c {File_location} < {test_cases_file} > {_output_path}",CancellationTokenP:ct);
            Runner.Start();
            using StreamReader STDERR = Runner.StandardError;
            try {
                await Task.Delay(timelimit * 1000 + 5000, ct.Token); // 5000ms due to any exit code  > 0 will cause the app to get a time to shut down so there might be a conflict between time limit and runtime
            }catch (Exception) {
                
                // just the code was too  fast :) ?? but did he pass?
            }
            if (ct.Token.IsCancellationRequested == false) {
                Runner.Kill();
                return "0x44"; //  time limit 
            }
            if (Runner.ExitCode == 0) {
                string answer = await ReadFileAsync(_output_path);
                return answer;
            }
            else{
                return "0x45"; // indicates an error occured (Runtime error)
            }  
        }
        public static string getReadyExtention(LanguagesEnum languages) {
            switch (languages) {
                case LanguagesEnum.cpp17:
                case LanguagesEnum.cpp14:
                    return ".exe";
                default:
                    return ".exe";
            }
        }
        public static string extractLanguageExtention(LanguagesEnum langauge) 
        {
            switch (langauge) {
                case LanguagesEnum.cpp17:
                case LanguagesEnum.cpp14:
                    return ".cpp";
                default:
                    return ".cpp";
            }
        }

        public static bool checkCompilationProcess(LanguagesEnum language)
        {
            switch (language)
            {
                case LanguagesEnum.cpp17:
                case LanguagesEnum.cpp14:
                    return true;
                default:
                    return true;
            }
        }
    }
}
