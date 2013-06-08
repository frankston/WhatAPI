using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
    internal class RangeSelector
    {
        public int RangeMin { get; private set; }
        public int RangeMax { get; private set; }
        public int DesiredSegmentSize { get; private set; }

        public int ActualSegmentSize { get; private set; }
        public int RandomRangeStart { get; private set; }
        public int RandomRangeEnd { get; private set; }

        public RangeSelector(int rangeMin, int rangeMax, int desiredSegmentSize)
        {
            this.RangeMin = rangeMin;
            this.RangeMax = rangeMax;
            this.DesiredSegmentSize = desiredSegmentSize;

            if (this.RangeMax > this.DesiredSegmentSize)
            {
                this.RandomRangeStart = Helper.GetRandomIntFromRange(this.RangeMin, this.RangeMax - this.DesiredSegmentSize);
                this.RandomRangeEnd = this.RandomRangeStart + this.DesiredSegmentSize;
            }
            else
            {
                this.RandomRangeStart = this.RangeMin;
                this.RandomRangeEnd = this.RangeMax;
            }
            this.ActualSegmentSize = this.RandomRangeEnd - this.RandomRangeStart;
        }
    }
}
