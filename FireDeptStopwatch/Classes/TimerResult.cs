using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireDeptStopwatch.Classes
{
    [Serializable]
    public class TimerResult
    {
        public DateTime DateTime { get; set; }

        public TimeSpan Result { get; set; }

        public int? Penalties { get; set; }

        public TimeSpan GetEndResult()
        {
            if (Penalties.HasValue)
            {
                var seconds = TimeSpan.FromSeconds(Penalties.Value);
                return Result.Add(seconds);
            }
            else
                return Result;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", new[] { DateTime.ToString("dd.MM.yyyy"), GetEndResult().ToString(@"mm\:ss\.ffff") });
        }
    }
}
