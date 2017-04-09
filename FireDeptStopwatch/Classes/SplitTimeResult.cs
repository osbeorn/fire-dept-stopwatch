using System;

namespace FireDeptStopwatch.Classes
{
    [Serializable]
    public class SplitTimeResult
    {
        public TimeSpan FromStart { get; set; }
        public TimeSpan FromPrevious { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", FromPrevious.ToString(@"mm\:ss\.ffff"), FromStart.ToString(@"mm\:ss\.ffff"));
        }
    }
}
