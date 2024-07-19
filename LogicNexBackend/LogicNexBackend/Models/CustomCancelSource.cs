using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicNexBackend.Models
{
    public class CustomCancellationSource : CancellationTokenSource
    {
        public bool Disposing { get; private set; } = false;
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Disposing = true;
        }
    }
}
