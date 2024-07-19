using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudgeSystem.Models
{
    internal class CustomCancellationSource : CancellationTokenSource
    {
        public bool Disposing { get; private set; } = false;
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Disposing = true;
        }
    }
}
