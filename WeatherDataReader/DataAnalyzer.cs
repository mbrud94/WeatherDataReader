using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WeatherDataReader
{
    public class DataAnalyzer
    {
         

        private List<MeteoData> allItems;
        private List<MeteoData> springItems = new List<MeteoData>();
        private List<MeteoData> summerItems = new List<MeteoData>();
        private List<MeteoData> autumnItems = new List<MeteoData>();
        private List<MeteoData> winterItems = new List<MeteoData>();

        public DataAnalyzer(List<MeteoData> allData)
        {
            this.allItems = allData;
            allItems.ForEach(i => AssignItemToSeason(i));
        }

        public void AnalyzeAllData()
        {
            StringBuilder info = new StringBuilder();
            AnalyzeDataPart(springItems, info);
            AnalyzeDataPart(summerItems, info);
            AnalyzeDataPart(autumnItems, info);
            AnalyzeDataPart(winterItems, info);
            AnalyzeDataPart(allItems, info);

            using (StreamWriter sw = new StreamWriter("statInfo.txt", true))
            {
                //sw.Write(info.ToString());
                sw.WriteLine(springItems.Count);
                sw.WriteLine(summerItems.Count);
                sw.WriteLine(autumnItems.Count);
                sw.WriteLine(winterItems.Count);
                sw.WriteLine(allItems.Count);

            }
            Console.WriteLine(info.ToString());

        }

        private void AssignItemToSeason(MeteoData keyItem)
        {
            DateTime springStart = new DateTime(2000, 3, 21);
            DateTime summerStart = new DateTime(2000, 6, 22);
            DateTime autumnStart = new DateTime(2000, 9, 23);
            DateTime winterStart = new DateTime(2000, 12, 22);

            springStart = new DateTime(keyItem.Date.Year, springStart.Month, springStart.Day);
            summerStart = new DateTime(keyItem.Date.Year, summerStart.Month, summerStart.Day);
            autumnStart = new DateTime(keyItem.Date.Year, autumnStart.Month, autumnStart.Day);
            winterStart = new DateTime(keyItem.Date.Year, winterStart.Month, winterStart.Day);
            if (keyItem.Date >= springStart && keyItem.Date < summerStart)
            {
                springItems.Add(keyItem);
            }
            else if (keyItem.Date >= summerStart && keyItem.Date < autumnStart)
            {
                summerItems.Add(keyItem);
            }
            else if (keyItem.Date >= autumnStart && keyItem.Date < winterStart)
            {
                autumnItems.Add(keyItem);
            }
            else if (keyItem.Date >= winterStart || keyItem.Date < springStart)
            {
                winterItems.Add(keyItem);
            }
        }

        private void AnalyzeDataPart(List<MeteoData> currentItems, StringBuilder info)
        {
            /*Func<MeteoData, double> selector = i => Double.Parse(i.TempMax.Replace('.', ','));
            info.Append($"Temperatura maksymalna\t{currentItems.Max(selector)}\t{currentItems.Min(selector)}\t{currentItems.Average(selector)}{Environment.NewLine}");
            selector = i => Double.Parse(i.TempMin.Replace('.', ','));
            info.Append($"Temperatura minimalna\t{currentItems.Max(selector)}\t{currentItems.Min(selector)}\t{currentItems.Average(selector)}{Environment.NewLine}");
            selector = i => Double.Parse(i.TempAvg.Replace('.', ','));
            info.Append($"Temperatura średnia\t{currentItems.Max(selector)}\t{currentItems.Min(selector)}\t{currentItems.Average(selector)}{Environment.NewLine}");
            selector = i => Double.Parse(i.CloudyAvg.Replace('.', ','));
            info.Append($"Zachmurzenie\t{currentItems.Max(selector)}\t{currentItems.Min(selector)}\t{currentItems.Average(selector)}{Environment.NewLine}");
            selector = i => Double.Parse(i.WindSpeed.Replace('.', ','));
            info.Append($"Prędkość wiatru\t{currentItems.Max(selector)}\t{currentItems.Min(selector)}\t{currentItems.Average(selector)}{Environment.NewLine}");
            selector = i => Double.Parse(i.WindDirection.Replace('.', ','));
            info.Append($"Kierunek wiatru\t{currentItems.Max(selector)}\t{currentItems.Min(selector)}\t{currentItems.Average(selector)}{Environment.NewLine}");
            selector = i => Double.Parse(i.PressureStationLevel.Replace('.', ','));
            info.Append($"Ciśnienie\t{currentItems.Max(selector)}\t{currentItems.Min(selector)}\t{currentItems.Average(selector)}{Environment.NewLine}");
            selector = i => Double.Parse(i.Rain12h.Replace('.', ','));
            info.Append($"Opad dobowy\t{currentItems.Max(selector)}\t{currentItems.Min(selector)}\t{currentItems.Average(selector)}{Environment.NewLine}");
            */
        }

    }

}
