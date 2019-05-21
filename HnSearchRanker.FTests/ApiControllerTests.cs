using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HnSearchRanker.ITests
{
    [TestClass]
    public class ApiControllerTests
    {
        private readonly WebApplicationFactory<HnSearchRanker.Startup> _factory;

        public ApiControllerTests()
        {
            _factory = new WebApplicationFactory<HnSearchRanker.Startup>();
        }

        [TestMethod]
        public async Task TestDistinctCount()
        {
            Assert.AreEqual(@"{""count"":573697}", await MakeDistinctCountRequest("2015"));
            Assert.AreEqual(@"{""count"":573697}", await MakeDistinctCountRequest("2015-08"));
            Assert.AreEqual(@"{""count"":198117}", await MakeDistinctCountRequest("2015-08-03"));
            Assert.AreEqual(@"{""count"":617}", await MakeDistinctCountRequest("2015-08-01 00:04"));
        }

        [TestMethod]
        public async Task TestTopQueries()
        {
            Assert.AreEqual(
                @"{""queries"":[{""query"":""http%3A%2F%2Fwww.getsidekick.com%2Fblog%2Fbody-language-advice"",""count"":6675}," +
                @"{""query"":""http%3A%2F%2Fwebboard.yenta4.com%2Ftopic%2F568045"",""count"":4652}," +
                @"{""query"":""http%3A%2F%2Fwebboard.yenta4.com%2Ftopic%2F379035%3Fsort%3D1"",""count"":3100}]}",
                await MakeTopQueriesRequest("2015", 3));

            Assert.AreEqual(
                @"{""queries"":[{""query"":""http%3A%2F%2Fwww.getsidekick.com%2Fblog%2Fbody-language-advice"",""count"":2283}," +
                @"{""query"":""http%3A%2F%2Fwebboard.yenta4.com%2Ftopic%2F568045"",""count"":1943}," +
                @"{""query"":""http%3A%2F%2Fwebboard.yenta4.com%2Ftopic%2F379035%3Fsort%3D1"",""count"":1358}," +
                @"{""query"":""http%3A%2F%2Fjamonkey.com%2F50-organizing-ideas-for-every-room-in-your-house%2F"",""count"":890}," +
                @"{""query"":""http%3A%2F%2Fsharingis.cool%2F1000-musicians-played-foo-fighters-learn-to-fly-and-it-was-epic"",""count"":701}]}",
                await MakeTopQueriesRequest("2015-08-02", 5));
        }

        private async Task<string> MakeDistinctCountRequest(string datePrefix)
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"/1/queries/count/{datePrefix}");
            return await response.Content.ReadAsStringAsync();
        }

        private async Task<string> MakeTopQueriesRequest(string datePrefix, int size)
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"/1/queries/popular/{datePrefix}?size={size}");
            return await response.Content.ReadAsStringAsync();
        }

    }
}
