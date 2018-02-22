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
        public DateTime Data { get; set; }
        public string TempMax { get; set; }
        public string TempMin { get; set; }
        public string TempSr { get; set; }
        public string TempSrL { get; set; }
        public string TempSrPunktuRosy { get; set; }
        public string TempMinPrzyGruncie { get; set; }
        public string TempMaxTermometruZwilzonego { get; set; }
        public string AnomaliaTempMax { get; set; }
        public string AnomaliaTempMin { get; set; }
        public string AnomaliaTempSr { get; set; }
        public string ZachmurzenieSr { get; set; }
        public string PredkoscWiatru { get; set; }
        public string KierunekWiatru { get; set; }
        public string ZachmurzenieSrL { get; set; }
        public string PredkoscWiatruL { get; set; }
        public string KierunekWiatruL { get; set; }
        //public string MaksymalnyPorywWiatru { get; set; }
        public string CisnienieSrPoziomMorza { get; set; }
        public string CisnienieSrPoziomStacji { get; set; }
        public string CisnienieSrPoziomStacjiL { get; set; }
        //public string OpadDobowy { get; set; }
        public string Opad12h { get; set; }
        public string Opad12hL { get; set; }
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

        public string TempSrWyL { get; set; }
        public string ZachmurzenieSrWyL { get; set; }
        public string PredkoscWiatruWyL { get; set; }
        public string KierunekWiatruWyL { get; set; }
        public string CisnienieSrPoziomStacjiWyL { get; set; }
        public string Opad12hWyL { get; set; }

        public MeteoData()
        {

        }

        public MeteoData(MeteoData rec)
        {
            this.Data = rec.Data;
            this.TempMax = rec.TempMax;
            this.TempMin = rec.TempMin;
            this.TempSr = rec.TempSr;
            this.TempSrPunktuRosy = rec.TempSrPunktuRosy;
            this.TempMinPrzyGruncie = rec.TempMinPrzyGruncie;
            this.TempMaxTermometruZwilzonego = rec.TempMaxTermometruZwilzonego;
            this.AnomaliaTempMax = rec.AnomaliaTempMax;
            this.AnomaliaTempMin = rec.AnomaliaTempMin;
            this.AnomaliaTempSr = rec.AnomaliaTempSr;
            this.ZachmurzenieSr = rec.ZachmurzenieSr;
            this.PredkoscWiatru = rec.PredkoscWiatru;
            this.KierunekWiatru = rec.KierunekWiatru;
            this.CisnienieSrPoziomMorza = rec.CisnienieSrPoziomMorza;
            this.CisnienieSrPoziomStacji = rec.CisnienieSrPoziomStacji;
            this.Opad12h = rec.Opad12h;
            this.SrWidzialnoscPozioma = rec.SrWidzialnoscPozioma;

            //LabelizeInput(s);
            //PrepareOutput(s);
        }

        public void PrepateOutputAndLabelize(Season s)
        {
            LabelizeInput(s);
            PrepareOutput(s);
        }

        private void LabelizeInput(Season season)
        {
            new Labelizator().LabelizeInput(this, season);
        }

        private void PrepareOutput(Season season)
        {
            KierunekWiatruWy = KierunekWiatru;
            PredkoscWiatruWy = PredkoscWiatru;
            ZachmurzenieSrWy = ZachmurzenieSr;
            Opad12hWy = Opad12h;
            TempSrWy = TempSr;
            CisnienieSrPoziomStacjiWy = CisnienieSrPoziomStacji;
            new Labelizator().LabelizeOutput(this, season);
        }

        public string ToInputString(bool labelize)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(TempMax)
                .Append(";")
                .Append(TempMin)
                .Append(";")
                .Append(labelize ? TempSrL : TempSr)
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
                .Append(labelize ? ZachmurzenieSrL : ZachmurzenieSr)
                .Append(";")
                .Append(labelize ? PredkoscWiatruL : PredkoscWiatru)
                .Append(";")
                .Append(labelize ? KierunekWiatruL : KierunekWiatru)
                .Append(";")
                .Append(CisnienieSrPoziomMorza)
                .Append(";")
                .Append(labelize ? CisnienieSrPoziomStacjiL : CisnienieSrPoziomStacji)
                .Append(";")
                .Append(labelize ? Opad12hL : Opad12h)
                .Append(";")
                .Append(SrWidzialnoscPozioma)
                .Append(";");

            return sb.ToString();
        }

        public string ToOutputString(bool labelize)
        {
            StringBuilder sb = new StringBuilder();
            if (!labelize)
            {
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
            }
            else
            {
                sb.Append(TempSrWyL)
                    .Append(";")
                    .Append(ZachmurzenieSrWyL)
                    .Append(";")
                    .Append(PredkoscWiatruWyL)
                    .Append(";")
                    .Append(KierunekWiatruWyL)
                    .Append(";")
                    .Append(CisnienieSrPoziomStacjiWyL)
                    .Append(";")
                    .Append(Opad12hWyL)
                    .Append(";");
            }
            return sb.ToString();
        }

    }
}
