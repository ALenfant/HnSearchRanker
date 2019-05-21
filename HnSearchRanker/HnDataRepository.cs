using System;
using System.Collections.Generic;
using System.Linq;

namespace HnSearchRanker
{
    public interface IHnDataRepository
    {
        int CountDistinctQueriesBetween(DateRange range);
        List<TopResult> TopQueriesBetween(DateRange range, int topCount);
    }

    public class HnDataRepository : IHnDataRepository
    {
        SortedSet<DateTime> _dateSet = new SortedSet<DateTime>();
        Dictionary<DateTime, List<string>> _queriesByDate = new Dictionary<DateTime, List<string>>();

        public void Add(DateTime dateTime, string query)
        {
            var dateTimeQueries = _queriesByDate.GetValueOrDefault(dateTime, new List<string>());
            dateTimeQueries.Add(query);
            _queriesByDate[dateTime] = dateTimeQueries;
            _dateSet.Add(dateTime);
        }

        public IEnumerable<string> GetQueriesBetween(DateRange range)
        {
            var endDate = range.End.AddTicks(-1); //Remove the smallest possible unit of time since the end is inclusive in GetViewBetween
            var subSet = _dateSet.GetViewBetween(range.Start, endDate);
            foreach (var dateTime in subSet)
            {
                foreach (var query in _queriesByDate[dateTime])
                {
                    yield return query;
                }
            }
        }

        public int CountDistinctQueriesBetween(DateRange range)
        {
            var queriesSet = new HashSet<string>();
            foreach (var query in GetQueriesBetween(range))
            {
                queriesSet.Add(query);
            }

            return queriesSet.Count;
        }

        public List<TopResult> TopQueriesBetween(DateRange range, int topCount)
        {
            //Count every time we encounter a query
            var queryCounts = new Dictionary<string, int>();
            foreach (var query in GetQueriesBetween(range))
            {
                var count = queryCounts.GetValueOrDefault(query, 0) + 1;
                queryCounts[query] = count;
            }

            //Sort the entries
            var sortedPairs = queryCounts.OrderByDescending(t => t.Value).Take(topCount);

            var results = new List<TopResult>();
            foreach (var keyValuePair in sortedPairs)
            {
                results.Add(new TopResult(keyValuePair.Key, keyValuePair.Value));
            }
            return results;
        }
    }
}
