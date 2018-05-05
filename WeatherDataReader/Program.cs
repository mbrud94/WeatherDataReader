using HtmlAgilityPack;
using Newtonsoft.Json;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace WeatherDataReader
{
    class Program
    {
        static string path =  $@"C:\Users\Mateusz\Desktop\Meteodata\"; //$@"D:\Studia\Mgr\Meteodata\";

        static void Main(string[] args)
        {
            string path = $@"C:\Users\Mateusz\Desktop\Meteodata\";
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("pl-PL");
            //1, 2:
            HtmlReader htmlReader = new HtmlReader(path, 2000, 2017);
            List<MeteoData> allItems = htmlReader.Read().OrderBy(i => i.Date).ToList();

            //new DataAnalyzer(allItems).AnalyzeAllData();
            //3:
            for (int groupSize = 1; groupSize <= 5; groupSize++)
            {
                //3.1:
                List<MeteoDataRecord> allRecords =
                    MeteoDataRecord.PrepareRecords(allItems, groupSize);
                //3.2:
                var springRecords = allRecords.Where(g => g.Season == Season.Spring).ToList();
                var summerRecords = allRecords.Where(g => g.Season == Season.Summer).ToList();
                var autumnRecords = allRecords.Where(g => g.Season == Season.Autumn).ToList();
                var winterRecords = allRecords.Where(g => g.Season == Season.Winter).ToList();
                //3.3:
                springRecords.ForEach(r => r.Labelize());
                summerRecords.ForEach(r => r.Labelize());
                autumnRecords.ForEach(r => r.Labelize());
                winterRecords.ForEach(r => r.Labelize());
                //3.4:
                MeteoDataSet springDS = new MeteoDataSet(springRecords, Season.Spring);
                MeteoDataSet summerDS = new MeteoDataSet(summerRecords, Season.Summer);
                MeteoDataSet autumnDS = new MeteoDataSet(autumnRecords, Season.Autumn);
                MeteoDataSet winterDS = new MeteoDataSet(winterRecords, Season.Winter);
                //3.5:
                List<MeteoDataSet> dataSets = new List<MeteoDataSet> { springDS, summerDS, autumnDS, winterDS };
                Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";
                foreach (var ds in dataSets)
                {
                    CsvSaver.SaveDataSet(ds, path, LabelizationMode.None);
                    CsvSaver.SaveDataSet(ds, path, LabelizationMode.Input);
                    CsvSaver.SaveDataSet(ds, path, LabelizationMode.Output);
                    CsvSaver.SaveDataSet(ds, path, LabelizationMode.Both);
                }
            }
            Console.WriteLine($"Empty records : {htmlReader.EmptyRecords}");
            Console.WriteLine("Done!");
            Console.ReadLine();
        }
    }
}
