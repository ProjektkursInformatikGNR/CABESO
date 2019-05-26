using System;
using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace CABESO
{
    public class Holidays
    {
        [JsonProperty("Neujahrstag")]
        public Holiday Neujahrstag { get; set; }
        [JsonProperty("Karfreitag")]
        public Holiday Karfreitag { get; set; }
        [JsonProperty("Ostermontag")]
        public Holiday Ostermontag { get; set; }
        [JsonProperty("Tag der Arbeit")]
        public Holiday TagDerArbeit { get; set; }
        [JsonProperty("Christi Himmelfahrt")]
        public Holiday ChristiHimmelfahrt { get; set; }
        [JsonProperty("Pfingstmontag")]
        public Holiday Pfingstmontag { get; set; }
        [JsonProperty("Fronleichnam")]
        public Holiday Fronleichnam { get; set; }
        [JsonProperty("Tag der Deutschen Einheit")]
        public Holiday TagDerDeutschenEinheit { get; set; }
        [JsonProperty("Allerheiligen")]
        public Holiday Allerheiligen { get; set; }
        [JsonProperty("1. Weihnachtstag")]
        public Holiday Weihnachtstag1 { get; set; }
        [JsonProperty("2. Weihnachtstag")]
        public Holiday Weihnachtstag2 { get; set; }

        public Holiday[] GetHolidays() => Array.ConvertAll(typeof(Holidays).GetProperties(), prop => prop.GetValue(this) as Holiday);

        public static Holidays Create()
        {
            if (!Program.HasInternetConnection())
                return null;
            HttpWebRequest request = WebRequest.Create($"https://feiertage-api.de/api/?jahr={DateTime.Now.Year}&nur_land=NW") as HttpWebRequest;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                return JsonConvert.DeserializeObject<Holidays>(reader.ReadToEnd());
        }
    }

    public class Holiday
    {
        [JsonProperty("datum")]
        private string _date { get; set; }
        [JsonProperty("hinweis")]
        public string Notes { get; set; }
        public DateTime Date { get => DateTime.Parse(_date); }
    }
}