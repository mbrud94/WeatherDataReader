using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace WeatherDataReader
{
    public class MeteoDataSet
    {
        private const double TEST_DATA_COUNT = 0.20;

        public List<MeteoDataGroup> TrainData { get; set; } = new List<MeteoDataGroup>();
        public List<MeteoDataGroup> TestData { get; set; } = new List<MeteoDataGroup>();

        public MeteoDataSet(List<MeteoDataGroup> allData)
        {
            int minYear = allData.Min(g => g.Output.Data.Year);
            int maxYear = allData.Max(g => g.Output.Data.Year);

            for (int year = minYear; year <= maxYear; year++)
            {
                var yearRecords = allData.Where(g => g.Output.Data.Year == year);
                int yearDataCount = yearRecords.Count();
                int testDataSize = (int)(TEST_DATA_COUNT * yearDataCount);

                Random rand = new Random();
                int idx = rand.Next(0, yearDataCount);

                List<int> drawIndexes = new List<int>();
                while(drawIndexes.Count <= testDataSize)
                {
                    if(!drawIndexes.Contains(idx))
                    {
                        drawIndexes.Add(idx);
                    }
                    idx = rand.Next(0, yearDataCount);
                }

                int s1 = TrainData.Count;
                int s2 = TestData.Count;

                TrainData.AddRange(yearRecords.Where((g, i) => !drawIndexes.Contains(i)));
                TestData.AddRange(yearRecords.Where((g, i) => drawIndexes.Contains(i)));

                if ((TestData.Count - s2) + (TrainData.Count - s1) != yearDataCount)
                    throw new Exception("Error during drawing");

            }
        }
    }
}
