using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireDeptStopwatch.Classes
{
    [Serializable]
    class TimerResult
    {
        public DateTime DateTime { get; set; }

        public TimeSpan Result { get; set; }

        public int Penalties { get; set; }

        public TimeSpan GetEndResult()
        {
            var seconds = TimeSpan.FromSeconds(Penalties);
            var endResult = Result.Add(seconds);

            return endResult;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", new[] { DateTime.ToString(), GetEndResult().ToString(@"mm\:ss\.ffff") });
        }
    }
}
