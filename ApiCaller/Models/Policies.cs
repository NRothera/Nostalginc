using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;

namespace Nostalginc.ApiCaller.Models
{
    public class Policies
    {
        public static RetryPolicy RetryAll()
        {
            return Policy.Handle<Exception>().WaitAndRetry(3, i => TimeSpan.FromSeconds(3));
        }

        public static RetryPolicy RetryAllExceptUnauthorised()
        {
            return Policy.Handle<ExternalException>().WaitAndRetry(3, i => TimeSpan.FromSeconds(3));
        }
    }
}
