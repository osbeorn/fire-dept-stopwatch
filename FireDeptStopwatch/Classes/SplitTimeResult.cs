using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FireDeptStopwatch.Classes
{
    [Serializable]
    public class SplitTimeResult
    {
        public TimeSpan Result { get; set; }

        public SplitTimeResult(TimeSpan result)
        {
            Result = result;
        }

        public override string ToString()
        {
            return string.Format("{0}", Result.ToString(@"mm\:ss\.ffff"));
        }
    }
}
