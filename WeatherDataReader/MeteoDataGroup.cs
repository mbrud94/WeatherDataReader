﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherDataReader
{
    public class MeteoDataGroup
    {
        public List<MeteoData> Inputs { get; set; }
        public MeteoData Output { get; set; }
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

        public string ToString(LabelizationMode labelizationMode)
        {
            bool labelizeInput = labelizationMode == LabelizationMode.Both || labelizationMode == LabelizationMode.Input;
            bool labelizeOutput = labelizationMode == LabelizationMode.Both || labelizationMode == LabelizationMode.Output;
            StringBuilder sb = new StringBuilder();
            foreach(var i in Inputs)
            {
                sb.Append(i.ToInputString(labelizeInput));
            }
            sb.Append(Output.ToOutputString(labelizeOutput));
            return sb.ToString().TrimEnd(';');
        }


        public static List<MeteoDataGroup> PrepareGroups(List<MeteoData> ungrupedData, int inputSize)
        {
            List<MeteoDataGroup> res = new List<MeteoDataGroup>();
            int notValidGroups = 0;

            for(int i = 0; i < ungrupedData.Count - inputSize - 1; i++)
            {
                List<MeteoData> input = new List<MeteoData>();
                for(int j = 0; j < inputSize; j++)
                {
                    input.Add(new MeteoData(ungrupedData[i + j]));
                }
                MeteoDataGroup group = new MeteoDataGroup { Inputs = input, Output = new MeteoData(ungrupedData[i + inputSize]) };
                if(IsGroupValid(group))
                {
                    res.Add(group);
                    group.SetSeason();
                    group.Inputs.ForEach(inpt => inpt.PrepateOutputAndLabelize(group.Season));
                    group.Output.PrepateOutputAndLabelize(group.Season);
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
            MeteoData prevInput = group.Inputs[0];
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
