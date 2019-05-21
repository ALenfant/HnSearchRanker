using System.Collections.Generic;

namespace HnSearchRanker.Responses
{
    public class TopResponse
    {
        public readonly List<TopResult> Queries;

        public TopResponse(List<TopResult> queries)
        {
            this.Queries = queries;
        }
    }
}