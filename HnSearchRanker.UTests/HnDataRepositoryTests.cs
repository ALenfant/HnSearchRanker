using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HnSearchRanker.UTests
{
    [TestClass]
    public class HnDataRepositoryTests
    {
        private HnDataRepository _hnDataRepository;

        [TestInitialize]
        public void SetUp()
        {
            _hnDataRepository = new HnDataRepository();
            _hnDataRepository.Add(DateTime.Parse("2015-08-01 11:23:53"), "Query");
            _hnDataRepository.Add(DateTime.Parse("2015-08-01 11:23:54"), "Query");
            _hnDataRepository.Add(DateTime.Parse("2015-08-01 11:23:54"), "Query2");
            _hnDataRepository.Add(DateTime.Parse("2015-08-01 11:23:55"), "Query");
            _hnDataRepository.Add(DateTime.Parse("2015-08-01 11:23:57"), "Query");
            _hnDataRepository.Add(DateTime.Parse("2015-08-01 11:23:57"), "Query3");
            _hnDataRepository.Add(DateTime.Parse("2015-08-01 11:23:58"), "Query2");
            _hnDataRepository.Add(DateTime.Parse("2015-08-01 11:23:59"), "Query");
            _hnDataRepository.Add(DateTime.Parse("2015-08-01 11:24:11"), "Query2");
        }

        [TestMethod]
        public void TestEndOfRangeExcluded()
        {
            var queries = _hnDataRepository.GetQueriesBetween(
                new DateRange(
                    DateTime.Parse("2015-08-01 11:23:53"),
                    DateTime.Parse("2015-08-01 11:23:54")
                )
            );

            Assert.AreEqual(1, queries.Count());
        }

        [TestMethod]
        public void TestCount()
        {
            var numQueries = _hnDataRepository.CountDistinctQueriesBetween(
                new DateRange(
                    DateTime.Parse("2015-08-01 11:23:53"),
                    DateTime.Parse("2015-08-01 11:23:55")
                )
            );

            Assert.AreEqual(2, numQueries);
        }

        [TestMethod]
        public void TestTop()
        {
            var top = _hnDataRepository.TopQueriesBetween(
                new DateRange(
                    DateTime.Parse("2015-08-01 11:23:53"),
                    DateTime.Parse("2015-08-01 11:24:11")
                ), 2
            );

            Assert.AreEqual(2, top.Count);
            Assert.AreEqual(new TopResult("Query", 5), top[0]);
            Assert.AreEqual(new TopResult("Query2", 2), top[1]);
        }

        [TestMethod]
        public void TestTopWithTooBigCount()
        {
            var top = _hnDataRepository.TopQueriesBetween(
                new DateRange(
                    DateTime.Parse("2015-08-01 11:23:53"),
                    DateTime.Parse("2015-08-01 11:24:11")
                ), 42
            );

            Assert.AreEqual(3, top.Count);
            Assert.AreEqual(new TopResult("Query", 5), top[0]);
            Assert.AreEqual(new TopResult("Query2", 2), top[1]);
            Assert.AreEqual(new TopResult("Query3", 1), top[2]);
        }
    }
}
