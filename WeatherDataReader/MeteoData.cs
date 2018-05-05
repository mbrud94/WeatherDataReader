using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherDataReader
{
    public enum Season
    {
        Spring,
        Summer,
        Autumn,
        Winter
    }

    public enum LabelizationMode
    {
        None,
        Input,
        Output,
        Both
    }

    public class MeteoData
    {
        public DateTime Date { get; set; }

        public double TempMax { get; set; }
        public double TempMin { get; set; }
        public double TempAvg { get; set; }
        public double CloudyAvg { get; set; }
        public double WindSpeed { get; set; }
        public double WindDirection { get; set; }
        public double PressureStationLevel { get; set; }
        public double Rain12h { get; set; }

        public int TempMaxL { get; set; }
        public int TempMinL { get; set; }
        public int TempAvgL { get; set; }
        public int CloudyAvgL { get; set; }
        public int WindSpeedL { get; set; }
        public int WindDirectionL { get; set; }
        public int PressureStationLevelL { get; set; }
        public int Rain12hL { get; set; }

        //public double TempMaxOut { get; set; }
        //public double TempMinOut { get; set; }
        //public double TempAvgOut { get; set; }
        //public double CloudyAvgOut { get; set; }
        //public double WindSpeedOut { get; set; }
        //public double WindDirectionOut { get; set; }
        //public double PressureStationLevelOut { get; set; }
        //public double Rain12hOut { get; set; }
        //
        //public int TempMaxOutL { get; set; }
        //public int TempMinOutL { get; set; }
        //public int TempAvgOutL { get; set; }
        //public int CloudyAvgOutL { get; set; }
        //public int WindSpeedOutL { get; set; }
        //public int WindDirectionOutL { get; set; }
        //public int PressureStationLevelOutL { get; set; }
        //public int Rain12hOutL { get; set; }

        //unused properties
        public double DevPointTempAvg { get; set; }
        public double GroundTempMin { get; set; }
        public double WettedThermMaxTemp { get; set; }
        public double TempMaxAnomaly { get; set; }
        public double TempMinAnomaly { get; set; }
        public double TempMAvgAnomaly { get; set; }
        public double MaxWindGust { get; set; }
        public double PressureSeeLevel { get; set; }
        public double Rain24h { get; set; }
        public double AvgVisibility { get; set; }
        public double Insolation { get; set; }
        public double IceSheetHeight { get; set; }
        public bool Storm { get; set; }
        public bool Rain { get; set; }
        public bool Snow { get; set; }
        public bool Fog { get; set; }


        public MeteoData()
        {

        }

        public MeteoData(MeteoData rec)
        {
            this.Date = rec.Date;
            this.TempMax = rec.TempMax;
            this.TempMin = rec.TempMin;
            this.TempAvg = rec.TempAvg;
            this.DevPointTempAvg = rec.DevPointTempAvg;
            this.GroundTempMin = rec.GroundTempMin;
            this.WettedThermMaxTemp = rec.WettedThermMaxTemp;
            this.TempMaxAnomaly = rec.TempMaxAnomaly;
            this.TempMinAnomaly = rec.TempMinAnomaly;
            this.TempMAvgAnomaly = rec.TempMAvgAnomaly;
            this.CloudyAvg = rec.CloudyAvg;
            this.WindSpeed = rec.WindSpeed;
            this.WindDirection = rec.WindDirection;
            this.PressureSeeLevel = rec.PressureSeeLevel;
            this.PressureStationLevel = rec.PressureStationLevel;
            this.Rain12h = rec.Rain12h;
            this.AvgVisibility = rec.AvgVisibility;

            //LabelizeInput(s);
            //PrepareOutput(s);
        }

        public void Labelize(Season s)
        {
            LabelizeInput(s);
            //PrepareOutput(s);
        }

        private void LabelizeInput(Season season)
        {
            new Labelizator().LabelizeRecord(this, season);
        }

        /*private void PrepareOutput(Season season)
        {
            WindDirectionOut = WindDirection;
            WindSpeedOut = WindSpeed;
            CloudyAvgOut = CloudyAvg;
            Rain12hOut = Rain12h;
            TempAvgOut = TempAvg;
            TempMaxOut = TempMax;
            TempMinOut = TempMin;
            PressureStationLevelOut = PressureStationLevel;
            new Labelizator().LabelizeOutput(this, season);
        }*/

        public static string GetHeader(int groupSize)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < groupSize; i++)
            {
                sb.Append("TempMax")
                    .Append(";")
                    .Append("TempMin")
                    .Append(";")
                    .Append("TempSr")
                    .Append(";")
                    //.Append("TempSrPunktuRosy")                 //
                    //.Append(";")                                //
                    //.Append("TempMinPrzyGruncie")               //
                    //.Append(";")                                //
                    //.Append("TempMaxTermometruZwilzonego")      //
                    //.Append(";")                                //
                    //.Append("AnomaliaTempMax")                  //
                    //.Append(";")                                //
                    //.Append("AnomaliaTempMin")                  //
                    //.Append(";")                                //
                    //.Append("AnomaliaTempSr")                   //
                    //.Append(";")                                //
                    .Append("ZachmurzenieSr")
                    .Append(";")
                    .Append("PredkoscWiatru")
                    .Append(";")
                    .Append("KierunekWiatru")
                    .Append(";")
                    //.Append("CisnienieSrPoziomMorza")//
                    //.Append(";")                     //
                    .Append("CisnienieSrPoziomStacji")
                    .Append(";")
                    .Append("Opad12h")
                    .Append(";");
                    //.Append("SrWidzialnoscPozioma")//
                    //.Append(";");                  //
                    
            }
            sb.Append("TempMaxWy")
              .Append(";")
              .Append("TempMinWy")
              .Append(";")
              .Append("TempSrWy")
              .Append(";")
              .Append("ZachmurzenieSrWy")
              .Append(";")
              .Append("PredkoscWiatruWy")
              .Append(";")
              .Append("KierunekWiatruWy")
              .Append(";")
              .Append("CisnienieSrPoziomStacjiWy")
              .Append(";")
              .Append("Opad12hWy");

            return sb.ToString();
        }

        public string ToInputString(bool labelize)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(labelize ? TempMaxL : TempMax)
                .Append(";")
                .Append(labelize ? TempMinL : TempMin)
                .Append(";")
                .Append(labelize ? TempAvgL : TempAvg)
                .Append(";")
                //.Append(TempSrPunktuRosy)              // 
                //.Append(";")                           //
                //.Append(TempMinPrzyGruncie)            //
                //.Append(";")                           //
                //.Append(TempMaxTermometruZwilzonego)   //
                //.Append(";")                           //
                //.Append(AnomaliaTempMax)               //
                //.Append(";")                           //
                //.Append(AnomaliaTempMin)               //
                //.Append(";")                           //
                //.Append(AnomaliaTempSr)                //
                //.Append(";")                           //
                .Append(labelize ? CloudyAvgL : CloudyAvg)
                .Append(";")
                .Append(labelize ? WindSpeedL : WindSpeed)
                .Append(";")
                .Append(labelize ? WindDirectionL : WindDirection)
                .Append(";")
                //.Append(CisnienieSrPoziomMorza)     //
                //.Append(";")                        //
                .Append(labelize ? PressureStationLevelL : PressureStationLevel)
                .Append(";")
                .Append(labelize ? Rain12hL : Rain12h)
                .Append(";");
                //.Append(SrWidzialnoscPozioma) //
                //.Append(";");                 //

            return sb.ToString();
        }

        public string ToOutputString(bool labelize)
        {
            return ToInputString(labelize);
            /*StringBuilder sb = new StringBuilder();
            if (!labelize)
            {
                sb.Append(TempMaxOut)
                    .Append(";")
                    .Append(TempMinOut)
                    .Append(";")
                    .Append(TempAvgOut)
                    .Append(";")
                    .Append(CloudyAvgOut)
                    .Append(";")
                    .Append(WindSpeedOut)
                    .Append(";")
                    .Append(WindDirectionOut)
                    .Append(";")
                    .Append(PressureStationLevelOut)
                    .Append(";")
                    .Append(Rain12hOut)
                    .Append(";");
            }
            else
            {
                sb.Append(TempMaxOutL)
                    .Append(";")
                    .Append(TempMinOutL)
                    .Append(";")
                    .Append(TempAvgOutL)
                    .Append(";")
                    .Append(CloudyAvgOut)
                    .Append(";")
                    .Append(WindSpeedOutL)
                    .Append(";")
                    .Append(WindDirectionOutL)
                    .Append(";")
                    .Append(PressureStationLevelOutL)
                    .Append(";")
                    .Append(Rain12hOutL)
                    .Append(";");
            }
            return sb.ToString();*/
        }

    }
}
