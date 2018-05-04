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
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("pl-PL");

            HtmlReader htmlReader = new HtmlReader(path, 2000, 2017);
            List<MeteoData> allItems = htmlReader.Read();

            //new DataAnalyzer(allItems).AnalyzeAllData();

            for (int groupSize = 1; groupSize <= 5; groupSize++)
            {
                allItems = allItems.OrderBy(i => i.Date).ToList();
                var groups = MeteoDataGroup.PrepareGroups(allItems, groupSize);
                MeteoDataSet springDS = new MeteoDataSet(groups.Where(g => g.Season == Season.Spring).ToList(), Season.Spring);
                MeteoDataSet summerDS = new MeteoDataSet(groups.Where(g => g.Season == Season.Summer).ToList(), Season.Summer);
                MeteoDataSet autumnDS = new MeteoDataSet(groups.Where(g => g.Season == Season.Autumn).ToList(), Season.Autumn);
                MeteoDataSet winterDS = new MeteoDataSet(groups.Where(g => g.Season == Season.Winter).ToList(), Season.Winter);
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
