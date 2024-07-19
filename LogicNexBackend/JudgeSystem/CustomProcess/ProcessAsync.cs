using JudgeSystem.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudgeSystem.CustomProcess
{
    internal class ProcessAsync : Process
    {
        private TaskCompletionSource<bool> _tcs = new TaskCompletionSource<bool>();
        private CustomCancellationSource? _CancellationToken = null;
        public ProcessAsync(string filename,string args, CustomCancellationSource? CancellationTokenP = null,bool input_redirect = false
            , bool output_redirect = false)
        {
            if (CancellationTokenP != null) { 
                _CancellationToken = CancellationTokenP;
            }
            EnableRaisingEvents = true;
            StartInfo = new()
            {
                FileName = filename,
                Arguments = args,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = output_redirect,
                RedirectStandardInput = input_redirect,
                RedirectStandardError = true,
            };
            Exited += (sender, args) =>
            {
                if (_CancellationToken != null && _CancellationToken.Disposing == false) {
                   _CancellationToken.Cancel();
                }
                _tcs.SetResult(true);
            };
        }
        public Task<bool> getTask()
        {
            return _tcs.Task;
        }
    }
}
