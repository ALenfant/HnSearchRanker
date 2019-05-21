using HnSearchRanker.Parser;
using HnSearchRanker.Responses;
using Microsoft.AspNetCore.Mvc;

namespace HnSearchRanker.Controllers
{
    [Route("1/queries")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private IHnDataRepository _hnDataRepository;

        public ApiController(IHnDataRepository hnDataRepository)
        {
            this._hnDataRepository = hnDataRepository;
        }

        [HttpGet("count/{datePrefix}")]
        public ActionResult<CountResponse> GetCount(string datePrefix)
        {
            var range = DatePrefixParser.Parse(datePrefix);
            var count = _hnDataRepository.CountDistinctQueriesBetween(range);
            return new CountResponse(count);
        }

        // GET api/values/5
        [HttpGet("popular/{datePrefix}")]
        public ActionResult<TopResponse> GetTop(string datePrefix, int size = 3)
        {
            var range = DatePrefixParser.Parse(datePrefix);
            var top = _hnDataRepository.TopQueriesBetween(range, size);
            return new TopResponse(top);
        }
    }
}
