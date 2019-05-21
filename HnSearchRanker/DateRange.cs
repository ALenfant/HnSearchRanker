using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HnSearchRanker
{
    public class DateRange
    {
        public readonly DateTime Start;
        public readonly DateTime End;

        public DateRange(DateTime start, DateTime end)
        {
            this.Start = start;
            this.End = end;
        }

        public override string ToString()
        {
            return $"DateRange from {Start} to {End}";
        }
    }
}
