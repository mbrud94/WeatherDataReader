using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherDataReader
{
    public class MeteoDataGroup
    {
        public List<MetoData> Inputs { get; set; }
        public MetoData Output { get; set; }
        public Season Season { get; set; }

        public void SetSeason()
        {
            DateTime springStart = new DateTime(2000, 3, 21);
            DateTime summerStart = new DateTime(2000, 6, 22);
            DateTime autumnStart = new DateTime(2000, 9, 23);
            DateTime winterStart = new DateTime(2000, 12, 22);

            var keyItem = Output; //assign by output

            springStart = new DateTime(keyItem.Data.Year, springStart.Month, springStart.Day);
            summerStart = new DateTime(keyItem.Data.Year, summerStart.Month, summerStart.Day);
            autumnStart = new DateTime(keyItem.Data.Year, autumnStart.Month, autumnStart.Day);
            winterStart = new DateTime(keyItem.Data.Year, winterStart.Month, winterStart.Day);
            if (keyItem.Data >= springStart && keyItem.Data < summerStart)
            {
                Season = Season.Spring;
            }
            else if (keyItem.Data >= summerStart && keyItem.Data < autumnStart)
            {
                Season = Season.Summer;
            }
            else if (keyItem.Data >= autumnStart && keyItem.Data < winterStart)
            {
                Season = Season.Autumn;
            }
            else if (keyItem.Data >= winterStart || keyItem.Data < springStart)
            {
                Season = Season.Winter;
            }

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach(var i in Inputs)
            {
                sb.Append(i.ToInputString());
            }
            sb.Append(Output.ToOutputString());
            return sb.ToString().TrimEnd(';');
        }


        public static List<MeteoDataGroup> PrepareGroups(List<MetoData> ungrupedData, int inputSize)
        {
            List<MeteoDataGroup> res = new List<MeteoDataGroup>();
            int notValidGroups = 0;

            for(int i = 0; i < ungrupedData.Count - inputSize - 1; i++)
            {
                List<MetoData> input = new List<MetoData>();
                for(int j = 0; j < inputSize; j++)
                {
                    input.Add(ungrupedData[i + j]);
                }
                MeteoDataGroup group = new MeteoDataGroup { Inputs = input, Output = ungrupedData[i + inputSize] };
                if(IsGroupValid(group))
                {
                    res.Add(group);
                    group.SetSeason();
                    group.Output.PrepareOutput(false, group.Season);
                }
                else
                {
                    notValidGroups++;
                }
            }

            Console.WriteLine("Not valid groups: " + notValidGroups);
            return res;
        }

        private static bool IsGroupValid(MeteoDataGroup group)
        {
            MetoData prevInput = group.Inputs[0];
            for(int i = 1; i < group.Inputs.Count; i++)
            {
                if (prevInput.Data.AddDays(1) != group.Inputs[i].Data)
                {
                    return false;
                }
                prevInput = group.Inputs[i];
            }
            if (prevInput.Data.AddDays(1) != group.Output.Data)
            {
                return false;
            }
            return true;
        }
    }
}
