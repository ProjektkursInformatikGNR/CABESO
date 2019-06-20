using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace CABESO
{
    /// <summary>
    /// Eine Sammlung der Feiertage in NRW (mithilfe der API des <see href="https://www.feiertage-api.de/">Feiertage Webservice</see>)
    /// </summary>
    public class Holidays
    {
        /// <summary>
        /// Neujahrstag (01.01.)
        /// </summary>
        [JsonProperty("Neujahrstag")]
        public Holiday Neujahrstag { get; set; }

        /// <summary>
        /// Karfreitag
        /// </summary>
        [JsonProperty("Karfreitag")]
        public Holiday Karfreitag { get; set; }

        /// <summary>
        /// Ostermontag
        /// </summary>
        [JsonProperty("Ostermontag")]
        public Holiday Ostermontag { get; set; }

        /// <summary>
        /// Tag der Arbeit (01.05.)
        /// </summary>
        [JsonProperty("Tag der Arbeit")]
        public Holiday TagDerArbeit { get; set; }

        /// <summary>
        /// Christi Himmelfahrt
        /// </summary>
        [JsonProperty("Christi Himmelfahrt")]
        public Holiday ChristiHimmelfahrt { get; set; }

        /// <summary>
        /// Pfingstmontag
        /// </summary>
        [JsonProperty("Pfingstmontag")]
        public Holiday Pfingstmontag { get; set; }

        /// <summary>
        /// Fronleichnam
        /// </summary>
        [JsonProperty("Fronleichnam")]
        public Holiday Fronleichnam { get; set; }

        /// <summary>
        /// Tag der Deutschen Einheit (03.10.)
        /// </summary>
        [JsonProperty("Tag der Deutschen Einheit")]
        public Holiday TagDerDeutschenEinheit { get; set; }

        /// <summary>
        /// Allerheiligen (01.11.)
        /// </summary>
        [JsonProperty("Allerheiligen")]
        public Holiday Allerheiligen { get; set; }

        /// <summary>
        /// 1. Weihnachtstag (25.12.)
        /// </summary>
        [JsonProperty("1. Weihnachtstag")]
        public Holiday Weihnachtstag1 { get; set; }

        /// <summary>
        /// 1. Weihnachtstag (26.12.)
        /// </summary>
        [JsonProperty("2. Weihnachtstag")]
        public Holiday Weihnachtstag2 { get; set; }

        /// <summary>
        /// Gibt alle zur Verfügung stehenden Feiertage im laufenden Kalendarjahr zurück.
        /// </summary>
        /// <returns>
        /// Diesjährige Feiertage
        /// </returns>
        public Holiday[] GetHolidays() => Array.ConvertAll(typeof(Holidays).GetProperties(), prop => prop.GetValue(this) as Holiday);

        /// <summary>
        /// Initialisiert die Eigenschaften vom Datentyp <see cref="Holiday"/> anhand der API des Feiertage Webservice.
        /// </summary>
        /// <returns>
        /// Ein mit aktuellen Daten befülltes <see cref="Holidays"/>-Objekt.
        /// </returns>
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

    /// <summary>
    /// Die Abbildung eines Feiertages
    /// </summary>
    public class Holiday
    {
        /// <summary>
        /// Das Datum des Feiertages im Format "yyyy-MM-dd"
        /// </summary>
        [JsonProperty("datum")]
        private string _date { get; set; }

        /// <summary>
        /// Ggf. zusätzliche Hinweise/Einschränkungen des Feiertages
        /// </summary>
        [JsonProperty("hinweis")]
        public string Notes { get; set; }

        /// <summary>
        /// Das als <see cref="DateTime"/> konvertierte Datum des Feiertages
        /// </summary>
        public DateTime Date { get => DateTime.Parse(_date); }
    }
}