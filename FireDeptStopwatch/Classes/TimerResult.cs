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
        public DateTime dateTime { get; set; }

        public TimeSpan result { get; set; }

        public int penalties { get; set; }

        public TimeSpan getEndResult()
        {
            var seconds = TimeSpan.FromSeconds(penalties);
            var endResult = result.Add(seconds);

            return endResult;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", new[] { dateTime.ToString(), getEndResult().ToString(@"mm\:ss\.ffff") });
        }
    }
}
