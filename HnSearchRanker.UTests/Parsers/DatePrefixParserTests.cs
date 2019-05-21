using System;
using HnSearchRanker.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HnSearchRanker.UTests.Parsers
{
    [TestClass]
    public class DatePrefixParserTests
    {
        [TestMethod]
        [DataRow("2015", "2015-01-01 00:00:00", "2016-01-01 00:00:00")]
        [DataRow("2015-08", "2015-08-01 00:00:00", "2015-09-01 00:00:00")]
        [DataRow("2015-08-03", "2015-08-03 00:00:00", "2015-08-04 00:00:00")]
        [DataRow("2015-08-02 01", "2015-08-02 01:00:00", "2015-08-02 02:00:00")]
        [DataRow("2015-08-01 00:04", "2015-08-01 00:04:00", "2015-08-01 00:05:00")]
        [DataRow("2015-08-03 11:13:26", "2015-08-03 11:13:26", "2015-08-03 11:13:27")]
        public void TestValidInputs(string input, string expectedStart, string expectedEnd)
        {
            var range = DatePrefixParser.Parse(input);
            Assert.AreEqual(DateTime.Parse(expectedStart), range.Start);
            Assert.AreEqual(DateTime.Parse(expectedEnd), range.End);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow("FALSE")]
        [DataRow("2")]
        [DataRow("20")]
        [DataRow("201")]
        [DataRow("2015-")]
        [DataRow("2015-1")]
        [DataRow("2015-11-")]
        [DataRow("2015-11-1")]
        [DataRow("2015-11-21 ")]
        [DataRow("2015-11-21 1")]
        [DataRow("2015-11-21 10:")]
        [DataRow("2015-11-21 10:1")]
        [DataRow("2015-11-21 10:11:")]
        [DataRow("2015-11-21 10:11:2")]
        public void TestInvalidInputs(string input)
        {
            Assert.ThrowsException<DatePrefixParsingException>(() => DatePrefixParser.Parse(input));
        }
    }
}
