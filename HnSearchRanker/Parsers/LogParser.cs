using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace HnSearchRanker.Parser
{
    public class LogParser
    {
        public static HnDataRepository Parse(string path)
        {
            var queryData = new HnDataRepository();


            using (FileStream compressedReader = File.OpenRead(path))
            {
                using (GZipStream zip = new GZipStream(compressedReader, CompressionMode.Decompress, true))
                {
                    using (StreamReader reader = new StreamReader(zip))
                    {
                        String line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parsedLine = ParseLogLine(line);
                            queryData.Add(parsedLine.Item1, parsedLine.Item2);
                        }
                    }
                }
            }







            return queryData;
        }

        public static (DateTime, string) ParseLogLine(string line)
        {
            var date = line.Substring(0, 19); //Standard date size is 19 characters
            var query = line.Substring(20); //The rest after the TAB is the query
            var dateTime = DateTime.Parse(date);

            return (dateTime, query);
        }
    }
}
