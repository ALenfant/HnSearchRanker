using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HnSearchRanker.Parser
{
    public class DatePrefixParser
    {
        private static string DateTemplate = "0001-01-01 00:00:00";
        private static string ExceptionValue = "DateTime prefix invalid!";

        public static DateRange Parse(string datePrefix)
        {
            //Transform the input into a full DateTime
            var datePrefixFilled = datePrefix + DateTemplate.Substring(datePrefix.Length);
            if (!DateTime.TryParse(datePrefixFilled, out var startDate))
            {
                throw new DatePrefixParsingException(ExceptionValue);
            }
            var endDate = FindEndDate(datePrefix, startDate);

            return new DateRange(
                startDate,
                endDate
            );
        }

        private static DateTime FindEndDate(string datePrefix, DateTime startDate)
        {
            switch (datePrefix.Length)
            {
                case 4: //Year provided
                    return startDate.AddYears(1);

                case 7: //Month provided
                    return startDate.AddMonths(1);

                case 10: //Day provided
                    return startDate.AddDays(1);

                case 13: //Hour provided
                    return startDate.AddHours(1);

                case 16: //Minutes provided
                    return startDate.AddMinutes(1);

                case 19: //Seconds provided
                    return startDate.AddSeconds(1);

                default:
                    throw new DatePrefixParsingException(ExceptionValue);
            }
        }
    }

    public class DatePrefixParsingException : Exception
    {
        public DatePrefixParsingException() {}

        public DatePrefixParsingException(string message) : base(message) {}

        public DatePrefixParsingException(string message, Exception inner) : base(message, inner) {}

    }
}
