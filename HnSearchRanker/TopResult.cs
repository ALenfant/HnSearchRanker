namespace HnSearchRanker
{
    public class TopResult
    {
        public readonly string Query;
        public readonly int Count;

        public TopResult(string query, int count)
        {
            this.Query = query;
            this.Count = count;
        }

        public override bool Equals(object other)
        {
            if (other == null || other.GetType() != this.GetType())
                return false;
            var otherCast = (TopResult) other;
            return this.Count == otherCast.Count && this.Query.Equals(otherCast.Query);
        }

        public override string ToString()
        {
            return $"{Query}: {Count}";
        }
    }
}