using System;
using HnSearchRanker.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HnSearchRanker.UTests.Parsers
{
    [TestClass]
    public class LogParserTests
    {
        [TestMethod]
        public void TestLogLineParsing()
        {
            var input = "2015-08-01 00:04:16\tslam";
            var parsedLine = LogParser.ParseLogLine(input);

            Assert.AreEqual(DateTime.Parse("2015-08-01 00:04:16"), parsedLine.Item1);
            Assert.AreEqual("slam", parsedLine.Item2);
        }
    }
}
