using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interview.Application.Exception
{
    public class ValidationException : ApplicationException
    {
        public ValidationException()
           : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }
        public IDictionary<string, string[]> Errors { get; }
    }
}
