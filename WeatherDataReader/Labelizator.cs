﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherDataReader
{
    public class Labelizator
    {
        public void LabelizeRecord(MeteoData item, Season season)
        {
            item.WindDirectionL = GetWindDirectionLabel(item.WindDirection);
            item.WindSpeedL = GetWindSpeedLabel(item.WindSpeed);
            item.CloudyAvgL = GetCloudyLabel(item.CloudyAvg);
            item.Rain12hL = GetRainLabel(item.Rain12h);
            item.TempAvgL = GetTempLabel(item.TempAvg, season);
            item.TempMinL = GetTempMinLabel(item.TempMin, season);
            item.TempMaxL = GetTempMaxLabel(item.TempMax, season);
            item.PressureStationLevelL = GetPreasureLabel(item.PressureStationLevel);
        }

        private int GetTempLabel(double temp, Season season)
        {
            switch (season)
            {
                case Season.Spring:
                    if (temp < 5)
                        return 1;
                    if (temp >= 5 && temp < 15)
                        return 2;
                    if (temp >= 15)
                        return 3;
                    throw new Exception("Invalid Temp");
                case Season.Summer:
                    if (temp < 10)
                        return 1;
                    if (temp >= 10 && temp < 20)
                        return 2;
                    if (temp >= 20)
                        return 3;
                    throw new Exception("Invalid Temp");
                case Season.Autumn:
                    if (temp < 0)
                        return 1;
                    if (temp >= 0 && temp < 10)
                        return 2;
                    if (temp >= 10)
                        return 3;
                    throw new Exception("Invalid Temp");
                case Season.Winter:
                    if (temp < -10)
                        return 1;
                    if (temp >= -10 && temp < 0)
                        return 2;
                    if (temp >= 0)
                        return 3;
                    throw new Exception("Invalid Temp");
                default:
                    throw new Exception("Invalid Season");
            }
        }

        private int GetTempMinLabel(double temp, Season season)
        {
            switch (season)
            {
                case Season.Spring:
                    if (temp < 0)
                        return 1;
                    if (temp >= 0 && temp < 10)
                        return 2;
                    if (temp >= 10)
                        return 3;
                    throw new Exception("Invalid Temp");
                case Season.Summer:
                    if (temp < 5)
                        return 1;
                    if (temp >= 5 && temp < 15)
                        return 2;
                    if (temp >= 15)
                        return 3;
                    throw new Exception("Invalid Temp");
                case Season.Autumn:
                    if (temp < -5)
                        return 1;
                    if (temp >= -5 && temp < 5)
                        return 2;
                    if (temp >= 5)
                        return 3;
                    throw new Exception("Invalid Temp");
                case Season.Winter:
                    if (temp < -15)
                        return 1;
                    if (temp >= -15 && temp < -5)
                        return 2;
                    if (temp >= -5)
                        return 3;
                    throw new Exception("Invalid Temp");
                default:
                    throw new Exception("Invalid Season");
            }
        }

        private int GetTempMaxLabel(double temp, Season season)
        {
            switch (season)
            {
                case Season.Spring:
                    if (temp < 10)
                        return 1;
                    if (temp >= 10 && temp < 20)
                        return 2;
                    if (temp >= 20)
                        return 3;
                    throw new Exception("Invalid Temp");
                case Season.Summer:
                    if (temp < 15)
                        return 1;
                    if (temp >= 15 && temp < 25)
                        return 2;
                    if (temp >= 25)
                        return 3;
                    throw new Exception("Invalid Temp");
                case Season.Autumn:
                    if (temp < 5)
                        return 1;
                    if (temp >= 5 && temp < 15)
                        return 2;
                    if (temp >= 15)
                        return 3;
                    throw new Exception("Invalid Temp");
                case Season.Winter:
                    if (temp < -5)
                        return 1;
                    if (temp >= -5 && temp < 5)
                        return 2;
                    if (temp >= 5)
                        return 3;
                    throw new Exception("Invalid Temp");
                default:
                    throw new Exception("Invalid Season");
            }
        }

        private int GetPreasureLabel(double preasure)
        {
            if (preasure < 990)
                return 1;
            if (preasure >= 990 && preasure < 1005)
                return 2;
            if (preasure >= 1005)
                return 3;
            throw new Exception("Invalid Preasure");
        }

        private int GetWindDirectionLabel(double windDirection)
        {
            windDirection = windDirection + 22.5;
            if (windDirection > 360)
            {
                windDirection = windDirection - 360;
            }
            if (windDirection >= 0 && windDirection < 45)
                return 1;
            if (windDirection >= 45 && windDirection < 90)
                return 2;
            if (windDirection >= 90 && windDirection < 135)
                return 3;
            if (windDirection >= 135 && windDirection < 180)
                return 4;
            if (windDirection >= 180 && windDirection < 225)
                return 5;
            if (windDirection >= 225 && windDirection < 270)
                return 6;
            if (windDirection >= 270 && windDirection < 315)
                return 7;
            if (windDirection >= 315 && windDirection <= 360)
                return 8;
            throw new Exception("Invalid wind direction");
        }

        private int GetWindSpeedLabel(double windSpeed)
        {
            if (windSpeed >= 0 && windSpeed < 3)
                return 1;
            if (windSpeed >= 3 && windSpeed < 6)
                return 2;
            if (windSpeed >= 6 && windSpeed < 9)
                return 3;
            if (windSpeed >= 9 && windSpeed < 12)
                return 4;
            if (windSpeed >= 12)
                return 5;
            throw new Exception("Invalid wind speed value");
        }

        private int GetCloudyLabel(double cloudy)
        {
            if (cloudy >= 0 && cloudy < 1)
                return 1;
            if (cloudy >= 1 && cloudy < 2)
                return 2;
            if (cloudy >= 2 && cloudy < 3)
                return 3;
            if (cloudy >= 3 && cloudy < 4)
                return 4;
            if (cloudy >= 4 && cloudy < 5)
                return 5;
            if (cloudy >= 5 && cloudy < 6)
                return 6;
            if (cloudy >= 6 && cloudy < 7)
                return 7;
            if (cloudy >= 7 && cloudy <= 8)
                return 8;
            throw new Exception("Invalid cloudy value");
        }

        private int GetRainLabel(double rain)
        {
            if (rain >= 0 && rain < 10)
                return 1;
            if (rain >= 10 && rain < 35)
                return 2;
            if (rain >= 35 && rain < 60)
                return 3;
            if (rain >= 60)
                return 4;
            throw new Exception("Invalid rain value");
        }

    }
}
