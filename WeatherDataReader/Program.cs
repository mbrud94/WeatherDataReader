using HtmlAgilityPack;
using Newtonsoft.Json;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace WeatherDataReader
{

    class Program
    {
        static string path =  $@"D:\Studia\Mgr\Meteodata\"; //$@"C:\Users\Mateusz\Desktop\Meteodata\";
        //static int GROUP_SIZE = 2;

        static void Main(string[] args)
        {
            allItems.Clear();
            //read
            for (int i = 2000; i<= 2017; i++)
                Read(i);
            //save not labelized
            //SaveToCsv(false); //only temp?

            //SaveToLabelizedCsv(false); //only temp?

            for (int groupSize = 1; groupSize <= 5; groupSize++)
            {
                allItems = allItems.OrderBy(i => i.Data).ToList();
                var groups = MeteoDataGroup.PrepareGroups(allItems, groupSize);
                MeteoDataSet springDS = new MeteoDataSet(groups.Where(g => g.Season == Season.Spring).ToList(), Season.Spring);
                MeteoDataSet summerDS = new MeteoDataSet(groups.Where(g => g.Season == Season.Summer).ToList(), Season.Summer);
                MeteoDataSet autumnDS = new MeteoDataSet(groups.Where(g => g.Season == Season.Autumn).ToList(), Season.Autumn);
                MeteoDataSet winterDS = new MeteoDataSet(groups.Where(g => g.Season == Season.Winter).ToList(), Season.Winter);
                List<MeteoDataSet> dataSets = new List<MeteoDataSet> { springDS, summerDS, autumnDS, winterDS };
                foreach (var ds in dataSets)
                {
                    CsvSaver.SaveDataSet(ds, path, LabelizationMode.None);
                    CsvSaver.SaveDataSet(ds, path, LabelizationMode.Input);
                    CsvSaver.SaveDataSet(ds, path, LabelizationMode.Output);
                    CsvSaver.SaveDataSet(ds, path, LabelizationMode.Both);
                }
            }
            //TODO: prapre CSV and labelization

            //Console.WriteLine($"Min spring avg temp : {allItems.Max(i => Double.Parse(i.TempSr.Replace('.', ',')))}");
            //Console.WriteLine($"Min spring avg temp : {allItems.Max(i => Double.Parse(i.TempMin.Replace('.', ',')))}");
            //Console.WriteLine($"Min spring avg temp : {allItems.Max(i => Double.Parse(i.TempMax.Replace('.', ',')))}");

            //Console.WriteLine($"Max wind speed : {allItems.Max(i => Double.Parse(i.PredkoscWiatru.Replace('.',',')))}");
            //Console.WriteLine($"Min wind speed : {allItems.Min(i => Double.Parse(i.PredkoscWiatru.Replace('.', ',')))}");
            //Console.WriteLine($"Max pressure : {allItems.Max(i => Double.Parse(i.CisnienieSrPoziomStacji.Replace('.', ',')))}");
            //Console.WriteLine($"Min pressure : {allItems.Min(i => Double.Parse(i.CisnienieSrPoziomStacji.Replace('.', ',')))}");
            //Console.WriteLine($"Max rain : {allItems.Max(i => Double.Parse(i.Opad12h.Replace('.',',')))}");
            //Console.WriteLine($"Min rain : {allItems.Min(i => Double.Parse(i.Opad12h.Replace('.', ',')))}");
            //Console.WriteLine($"Min spring avg temp : {springItems.Min(i => Double.Parse(i.TempSr.Replace('.',',')))}");
            //Console.WriteLine($"Max spring avg temp : {springItems.Max(i => Double.Parse(i.TempSr.Replace('.',',')))}");
            //Console.WriteLine($"Min summer avg temp : {summerItems.Min(i => Double.Parse(i.TempSr.Replace('.',',')))}");
            //Console.WriteLine($"Max summer avg temp : {summerItems.Max(i => Double.Parse(i.TempSr.Replace('.',',')))}");
            //Console.WriteLine($"Min autumn avg temp : {autumnItems.Min(i => Double.Parse(i.TempSr.Replace('.',',')))}");
            //Console.WriteLine($"Max autumn avg temp : {autumnItems.Max(i => Double.Parse(i.TempSr.Replace('.',',')))}");
            //Console.WriteLine($"Min winter avg temp : {winterItems.Min(i => Double.Parse(i.TempSr.Replace('.',',')))}");
            //Console.WriteLine($"Max winter avg temp : {winterItems.Max(i => Double.Parse(i.TempSr.Replace('.',',')))}");
            Console.WriteLine($"Empty records : {emptyRecords}");
            Console.WriteLine("Done!");
            Console.ReadLine();
        }

        private static List<MeteoDataGroup> springItems = new List<MeteoDataGroup>();
        private static List<MeteoDataGroup> summerItems = new List<MeteoDataGroup>();
        private static List<MeteoDataGroup> autumnItems = new List<MeteoDataGroup>();
        private static List<MeteoDataGroup> winterItems = new List<MeteoDataGroup>();

        private static List<MeteoData> allItems = new List<MeteoData>();

        private static int emptyRecords = 0;

        static void Read(int year)
        {
            //PrepareFile(year);
            ReadFromHtml(year, false);
        }

        static void PrepareFile(int year)
        {
            foreach (var file in Directory.GetFiles($@"{path}{year}\FullHtml"))
            {
                if (file.EndsWith(".html"))
                {
                    string fileName = file.Substring(file.LastIndexOf('\\') + 1);
                    using (StreamReader sr = new StreamReader(file))
                    {
                        string content = sr.ReadToEnd();
                        int start = content.IndexOf("<table id=\"tablepl\"");
                        int end = content.IndexOf("</table>", start);
                        string table = content.Substring(start, end - start);
                        if (!Directory.Exists($@"{path}{year}\OnlyTables"))
                        {
                            Directory.CreateDirectory($@"{path}{year}\OnlyTables");
                        }
                        using (StreamWriter sw = new StreamWriter($@"{path}{year}\OnlyTables\" + fileName))
                        {
                            sw.Write(table);
                        }
                    }
                }
            }

        }

        static void ReadFromHtml(int year, bool serialize)
        {
            List<MeteoData> data = new List<MeteoData>();

            foreach (var file in Directory.GetFiles($@"{path}{year}\OnlyTables"))
            {
                if (file.EndsWith(".html"))
                {
                    HtmlDocument doc = new HtmlDocument();
                    doc.Load(file);

                    int counter = 0;
                    foreach (HtmlNode row in doc.DocumentNode.SelectNodes("//table[@id='tablepl']//tr"))
                    {
                        if (counter++ < 2)
                        {
                            continue;
                        }
                        MeteoData meteoRecord = new MeteoData();

                        bool save = true;
                        int cellIdx = 1;
                        foreach (var cell in row.SelectNodes(".//td"))
                        {
                            if(cellIdx == 1 && DateTime.Parse(cell.FirstChild.InnerText).Year != year)
                            {
                                save = false;
                                break;
                            }
                            if(cellIdx == 14)
                            {
                                if(!SaveCellValue(cellIdx, meteoRecord, MapWindProperty(cell)))
                                {
                                    save = false;
                                    emptyRecords++;
                                    break;
                                }
                            }
                            else if (cellIdx <= 20 && cellIdx != 2 && cellIdx != 15 && cellIdx != 18)//14
                            {
                                if(!SaveCellValue(cellIdx, meteoRecord, cell.FirstChild.InnerText.Trim()))
                                {
                                    save = false;
                                    emptyRecords++;
                                    break;
                                }
                            }
                            /*else
                            {
                                SaveCellValue(cellIdx, meteoRecord, MapImgProperty(cell));
                            }*/
                            cellIdx++;
                        }
                        if(save && !data.Any(r => r.Data == meteoRecord.Data))
                        {
                            data.Add(meteoRecord);
                        }
                    }
                }
            }
            //data = data.OrderBy(d => d.Data).ToList();
            if (serialize)
            {
                SaveToJson(data, year);
            }
            //AssignItems(data);
            //summerItems.AddRange(data);
            Console.WriteLine($"Data for {year} saved. Records: {data.Count}");
            //JsonToCSV(data, ";", year);

            allItems.AddRange(data);
        }

        private static void SaveToJson(List<MeteoData> data, int year)
        {
            File.WriteAllText($@"{path}PreparedData\MeteoData_{year}.json", JsonConvert.SerializeObject(data, Formatting.Indented));
            /*using (StreamWriter file = File.CreateText($@"{path}{year}\data.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, data);
            }*/
        }

        private static void SaveToCsv(bool onlyTemp = false)
        {
            ServiceStack.Text.CsvConfig.ItemSeperatorString = ";";
            string tempNamePart = onlyTemp ? "_OnlyTemp" : string.Empty;
            using (StreamWriter sw = new StreamWriter($@"{path}PreparedData\MeteoData{tempNamePart}_Spring.csv"))
            {
                CsvSerializer.SerializeToWriter(springItems, sw);
            }
            using (StreamWriter sw = new StreamWriter($@"{path}PreparedData\MeteoData{tempNamePart}_Summer.csv"))
            {
                CsvSerializer.SerializeToWriter(summerItems, sw);
            }
            using (StreamWriter sw = new StreamWriter($@"{path}PreparedData\MeteoData{tempNamePart}_Autumn.csv"))
            {
                CsvSerializer.SerializeToWriter(autumnItems, sw);
            }
            using (StreamWriter sw = new StreamWriter($@"{path}PreparedData\MeteoData{tempNamePart}_Winter.csv"))
            {
                CsvSerializer.SerializeToWriter(winterItems, sw);
            }
        }

        private static void SaveToLabelizedCsv(bool onlyTemp = false)
        {
            string tempNamePart = onlyTemp ? "_OnlyTemp" : string.Empty;
            ServiceStack.Text.CsvConfig.ItemSeperatorString = ";";
            using (StreamWriter sw = new StreamWriter($@"{path}PreparedData\MeteoData{tempNamePart}_Spring_Labelized.csv"))
            {
                CsvSerializer.SerializeToWriter(springItems, sw);
            }
            using (StreamWriter sw = new StreamWriter($@"{path}PreparedData\MeteoData{tempNamePart}_Summer_Labelized.csv"))
            {
                CsvSerializer.SerializeToWriter(summerItems, sw);
            }
            using (StreamWriter sw = new StreamWriter($@"{path}PreparedData\MeteoData{tempNamePart}_Autumn_Labelized.csv"))
            {
                CsvSerializer.SerializeToWriter(autumnItems, sw);
            }
            using (StreamWriter sw = new StreamWriter($@"{path}PreparedData\MeteoData{tempNamePart}_Winter_Labelized.csv"))
            {
                CsvSerializer.SerializeToWriter(winterItems, sw);
            }
        }

        private static string MapImgProperty(HtmlNode cell)
        {
            string classAtr = cell.FirstChild.GetAttributeValue("class", "");
            switch(classAtr)
            {
                case "fa fa-circle": return "1";
                case "fa fa-circle-o": return "0";
            }
            throw new ArgumentException($"Not mapping for class {classAtr}");

        }

        private static List<string> imgSources = new List<string>();
        private static string MapWindProperty(HtmlNode cell)
        {
            //string src = cell.FirstChild.GetAttributeValue("src", "");
            double transform = Convert.ToDouble(cell.FirstChild.GetAttributeValue("data-rotate", "").Replace('.',','));
            if (transform >= 270) transform = transform - 270;
            else if (transform != 0) transform = transform + 90;
            string ret = Math.Round(transform, 2).ToString().Replace(',', '.');
            return ret;
        }

        private static bool SaveCellValue(int cellIdx, MeteoData row, string value)
        {
            //value = value.Replace('.', ',');
            value = value.Replace(',', '.');
            if (value == string.Empty || value == "-")
            {
                value = string.Empty;
            }
            if(string.IsNullOrEmpty(value))
            {
                return false;
            }
            switch(cellIdx)
            {
                case 1: row.Data = DateTime.Parse(value); break;
                //case 2: row.N = value; break;
                case 3: row.TempMax = value; break;
                case 4: row.TempMin = value; break;
                case 5: row.TempSr = value; break;
                case 6: row.TempSrPunktuRosy = value; break;
                case 7: row.TempMinPrzyGruncie = value; break;
                case 8: row.TempMaxTermometruZwilzonego = value; break;
                case 9: row.AnomaliaTempMax = value; break;
                case 10: row.AnomaliaTempMin = value; break;
                case 11: row.AnomaliaTempSr = value; break;
                case 12: row.ZachmurzenieSr = value; break;
                case 13: row.PredkoscWiatru = value; break;
                case 14: row.KierunekWiatru = value; break;
                //case 15: row.MaksymalnyPorywWiatru = value; break;
                case 16: row.CisnienieSrPoziomMorza = value; break;
                case 17: row.CisnienieSrPoziomStacji = value; break;
                //case 18: row.OpadDobowy = value; break;
                case 19: row.Opad12h = value; break;
                case 20: row.SrWidzialnoscPozioma = value; break;
                //case 21: row.Uslonecznienie = value; break;
                //case 22: row.WysokoscPokrywySnieznej = value; break;
                //case 23: row.Burza = value; break;
                //case 24: row.Deszcz = value; break;
                //case 25: row.Snieg = value; break;
                //case 26: row.Mgla = value; break;
            }
            return true;
        }

        

        /*private class MetoData
        {
            public DateTime Data; //{ get; set; }
            //public string N { get; set; }
            public string TempMax { get; set; }
            public string TempMin { get; set; }
            public string TempSr { get; set; }
            public string TempSrPunktuRosy { get; set; }
            public string TempMinPrzyGruncie { get; set; }
            public string TempMaxTermometruZwilzonego { get; set; }
            public string AnomaliaTempMax { get; set; }
            public string AnomaliaTempMin { get; set; }
            public string AnomaliaTempSr { get; set; }
            public string ZachmurzenieSr; //{ get; set; }
            public string PredkoscWiatru; //{ get; set; }
            public string KierunekWiatru; //{ get; set; }
            //public string MaksymalnyPorywWiatru { get; set; }
            public string CisnienieSrPoziomMorza; //{ get; set; }
            public string CisnienieSrPoziomStacji;// { get; set; }
            //public string OpadDobowy { get; set; }
            public string Opad12h;// { get; set; }
            public string SrWidzialnoscPozioma;// { get; set; }
            //public string Uslonecznienie { get; set; }
            //public string WysokoscPokrywySnieznej { get; set; }
            //public string Burza { get; set; }
            //public string Deszcz { get; set; }
            //public string Snieg { get; set; }
            //public string Mgla { get; set; }

            public string TempSrWy { get; set; }
            public string ZachmurzenieSrWy;// { get; set; }
            public string PredkoscWiatruWy;// { get; set; }
            public string KierunekWiatruWy;// { get; set; }
            public string CisnienieSrPoziomStacjiWy;// { get; set; }
            public string Opad12hWy;// { get; set; }

        }*/
    }
}
