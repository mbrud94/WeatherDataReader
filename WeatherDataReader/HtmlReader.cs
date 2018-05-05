using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WeatherDataReader
{
    public class HtmlReader
    {
        private static List<int> KEY_PROPERTIES = new List<int> {1, 3, 4, 5, 12, 13, 14, 17, 19 };

        public int EmptyRecords { get; set; } = 0;

        private string path;
        private int yearFrom;
        private int yearTo;

        private List<MeteoData> allItems;

        public HtmlReader(string path, int yearFrom, int yearTo)
        {
            this.path = path;
            this.yearFrom = yearFrom;
            this.yearTo = yearTo;
        }

        public List<MeteoData> Read()
        {
            this.allItems = new List<MeteoData>();
            for (int i = yearFrom; i <= yearTo; i++)
            {
                ExtractHtmlTablesFromFiles(i);
                ReadFromHtml(i);
            }
            return allItems;
        }

        private void ExtractHtmlTablesFromFiles(int year)
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

        private void ReadFromHtml(int year)
        {
            int readRecords = 0;
            foreach (var file in Directory.GetFiles($@"{path}{year}\OnlyTables"))
            {
                if (file.EndsWith(".html"))
                {
                    HtmlDocument doc = new HtmlDocument();
                    doc.Load(file);

                    int rowCounter = 0;
                    var allRows = doc.DocumentNode.SelectNodes("//table[@id='tablepl']//tr");
                    foreach (HtmlNode row in allRows)
                    {
                        if (rowCounter++ < 2) //skip table headers
                            continue;

                        MeteoData meteoRecord = new MeteoData();

                        bool save = true;
                        int cellIdx = 1;
                        foreach (var cell in row.SelectNodes(".//td"))
                        {
                            if (!KEY_PROPERTIES.Contains(cellIdx))
                            {
                                cellIdx++;
                                continue;
                            }

                            string cellValue;
                            if (cellIdx == 14)
                                cellValue = MapWindProperty(cell);
                            else if (cellIdx > 20)
                                cellValue = MapImgProperty(cell);
                            else
                                cellValue = cell.FirstChild.InnerText.Trim();

                            if (!StoreCellValue(cellIdx, meteoRecord, cellValue))
                            {
                                save = false;
                                EmptyRecords++;
                                break;
                            }                            
                            cellIdx++;
                        }
                        if (save && !allItems.Any(r => r.Date == meteoRecord.Date))
                        {
                            readRecords++;
                            allItems.Add(meteoRecord);
                        }
                    }
                }
            }
            Console.WriteLine($"Data for {year} read. Records: {readRecords}");
        }



        private bool StoreCellValue(int cellIdx, MeteoData row, string value)
        {
            value = value.Replace('.', ',');

            if (value == string.Empty || value == "-")
                value = string.Empty;

            if (string.IsNullOrEmpty(value))
                return false;
            switch (cellIdx)
            {
                case 1: row.Date = DateTime.Parse(value); break;
                case 3: row.TempMax = Double.Parse(value); break;
                case 4: row.TempMin = Double.Parse(value); break;
                case 5: row.TempAvg = Double.Parse(value); break;
                case 6: row.DevPointTempAvg = Double.Parse(value); break;
                case 7: row.GroundTempMin = Double.Parse(value); break;
                case 8: row.WettedThermMaxTemp = Double.Parse(value); break;
                case 9: row.TempMaxAnomaly = Double.Parse(value); break;
                case 10: row.TempMinAnomaly = Double.Parse(value); break;
                case 11: row.TempMAvgAnomaly = Double.Parse(value); break;
                case 12: row.CloudyAvg = Double.Parse(value); break;
                case 13: row.WindSpeed = Double.Parse(value); break;
                case 14: row.WindDirection = Double.Parse(value); break;
                case 15: row.MaxWindGust = Double.Parse(value); break;
                case 16: row.PressureSeeLevel = Double.Parse(value); break;
                case 17: row.PressureStationLevel = Double.Parse(value); break;
                case 18: row.Rain24h = Double.Parse(value); break;
                case 19: row.Rain12h = Double.Parse(value); break;
                case 20: row.AvgVisibility = Double.Parse(value); break;
                case 21: row.Insolation = Double.Parse(value); break;
                case 22: row.IceSheetHeight = Double.Parse(value); break;
                case 23: row.Storm = Double.Parse(value) == 1.0; break;
                case 24: row.Rain = Double.Parse(value) == 1.0; break;
                case 25: row.Snow = Double.Parse(value) == 1.0; break;
                case 26: row.Fog = Double.Parse(value) == 1.0; break;
            }
            return true;
        }

        private string MapWindProperty(HtmlNode cell)
        {
            double transform = Convert.ToDouble(cell.FirstChild.GetAttributeValue("data-rotate", "").Replace('.', ','));
            if (transform >= 270) transform = transform - 270;
            else if (transform != 0) transform = transform + 90;
            string ret = Math.Round(transform, 2).ToString().Replace(',', '.');
            return ret;
        }

        private string MapImgProperty(HtmlNode cell)
        {
            string classAtr = cell.FirstChild.GetAttributeValue("class", "");
            switch (classAtr)
            {
                case "fa fa-circle": return "1";
                case "fa fa-circle-o": return "0";
            }
            throw new ArgumentException($"Not mapping for class {classAtr}");
        }
    }
}
