using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecuteProcess.ServiceEnt
{
    public class ResponseTask
    {       
        public bool Succes { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }

        public string Result { get; set; }

        public ResponseTask()
        {
            this.Succes = false;
            this.Message = String.Empty;
            this.StackTrace = string.Empty;
        }
    }
}
