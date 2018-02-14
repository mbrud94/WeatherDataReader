using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WeatherDataReader
{
    public class CsvSaver
    {
        private const string DS_TYPE = "DS_TYPE";

        public static void SaveDataSet(MeteoDataSet dataSet, string path)
        {
            var groupSize = dataSet.TrainData[0].Inputs.Count;
            string directoryPart = $@"{path}PreparedData\{groupSize}\Labelization_{dataSet.LabelizationMode.ToString()}\";
            string filePart = $"{dataSet.Season.ToString()}_{DS_TYPE}.csv";
            var pathToSave = directoryPart + filePart;
            Directory.CreateDirectory(directoryPart);
            using (StreamWriter sw = new StreamWriter(pathToSave.Replace(DS_TYPE, "Train")))
            {
                foreach(var gr in dataSet.TrainData)
                {
                    sw.WriteLine(gr.ToString());
                }
            }
            using (StreamWriter sw = new StreamWriter(pathToSave.Replace(DS_TYPE, "Test")))
            {
                foreach (var gr in dataSet.TestData)
                {
                    sw.WriteLine(gr.ToString());
                }
            }

        }
    }
}
