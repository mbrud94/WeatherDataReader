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

    public class MetoData
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
        public string ZachmurzenieSr { get; set; }
        public string PredkoscWiatru { get; set; }
        public string KierunekWiatru { get; set; }
        //public string MaksymalnyPorywWiatru { get; set; }
        public string CisnienieSrPoziomMorza { get; set; }
        public string CisnienieSrPoziomStacji { get; set; }
        //public string OpadDobowy { get; set; }
        public string Opad12h { get; set; }
        public string SrWidzialnoscPozioma { get; set; }
        //public string Uslonecznienie { get; set; }
        //public string WysokoscPokrywySnieznej { get; set; }
        //public string Burza { get; set; }
        //public string Deszcz { get; set; }
        //public string Snieg { get; set; }
        //public string Mgla { get; set; }

        public string TempSrWy { get; set; }
        public string ZachmurzenieSrWy { get; set; }
        public string PredkoscWiatruWy { get; set; }
        public string KierunekWiatruWy { get; set; }
        public string CisnienieSrPoziomStacjiWy { get; set; }
        public string Opad12hWy { get; set; }

        public void PrepareOutput(bool labelize, Season season)
        {
            if(!labelize)
            {
                KierunekWiatruWy = KierunekWiatru;
                PredkoscWiatruWy = PredkoscWiatru;
                ZachmurzenieSrWy = ZachmurzenieSr;
                Opad12hWy = Opad12h;
                TempSrWy = TempSr;
                CisnienieSrPoziomStacjiWy = CisnienieSrPoziomStacji;
            }
            else
            {
                new Labelizator().LabelizeOutput(this, season);
            }
        }

        public string ToInputString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(TempMax)
                .Append(";")
                .Append(TempMin)
                .Append(";")
                .Append(TempSr)
                .Append(";")
                .Append(TempSrPunktuRosy)
                .Append(";")
                .Append(TempMinPrzyGruncie)
                .Append(";")
                .Append(TempMaxTermometruZwilzonego)
                .Append(";")
                .Append(AnomaliaTempMax)
                .Append(";")
                .Append(AnomaliaTempMin)
                .Append(";")
                .Append(AnomaliaTempSr)
                .Append(";")
                .Append(ZachmurzenieSr)
                .Append(";")
                .Append(PredkoscWiatru)
                .Append(";")
                .Append(KierunekWiatru)
                .Append(";")
                .Append(CisnienieSrPoziomMorza)
                .Append(";")
                .Append(CisnienieSrPoziomStacji)
                .Append(";")
                .Append(Opad12h)
                .Append(";")
                .Append(SrWidzialnoscPozioma)
                .Append(";");

            return sb.ToString();
        }

        public string ToOutputString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(TempSrWy)
                .Append(";")
                .Append(ZachmurzenieSrWy)
                .Append(";")
                .Append(PredkoscWiatruWy)
                .Append(";")
                .Append(KierunekWiatruWy)
                .Append(";")
                .Append(CisnienieSrPoziomStacjiWy)
                .Append(";")
                .Append(Opad12hWy)
                .Append(";");

            return sb.ToString();
        }

    }
}
