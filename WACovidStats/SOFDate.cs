using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WACovidStats
{
    public class SOFDate
    {
        public static void Execute()
        {
            var client = new RestClient("https://services.arcgis.com/Qxcws3oU4ypcnx4H/arcgis/rest/services/Epidemic_curve_date_new_view_layer/FeatureServer/0/query?f=json&where=Total_Confirmed%20IS%20NOT%20NULL&returnGeometry=false&spatialRel=esriSpatialRelIntersects&outFields=*&orderByFields=Date%20asc&outSR=102100&resultOffset=0&resultRecordCount=32000&resultType=standard&cacheHint=true");
            client.Timeout = -1;
            client.UseNewtonsoftJson();
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            JObject des = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
            List<SourceInfectionDate> list = new List<SourceInfectionDate>();
            foreach (var it in des["features"])
            {
                var item = it.First().First();
                SourceInfectionDate stat = new SourceInfectionDate();
                stat.Date = (string)((JValue)item["Date"]).Value.ToString();
                long unixmil = long.Parse(stat.Date);
                var itdate = DateTimeOffset.FromUnixTimeMilliseconds(unixmil);
                stat.Date = itdate.Date.ToShortDateString();
                stat.FID = (string)((JValue)item["FID"]).Value.ToString();
                stat.CruiseShip = (string)((JValue)item["Cruise_Ships"]).Value?.ToString();
                stat.CloseContact = (string)((JValue)item["Close_Contact"]).Value?.ToString();
                stat.Interstate = (string)((JValue)item["Interstate"]).Value?.ToString();
                stat.Overseas = (string)((JValue)item["Oversea_Travel"]).Value?.ToString();
                stat.Pending = (string)((JValue)item["Pending"]).Value?.ToString();
                stat.TotalConfirmed = (string)((JValue)item["Total_Confirmed"]).Value?.ToString();
                stat.Unknown = (string)((JValue)item["Unknown"]).Value?.ToString();
                list.Add(stat);
            }
            JsonObject item2 = new JsonObject();
            //item2.Add("date_updated", latestdate);
            item2.Add("data", list);
            File.WriteAllText("docs/source_date.json", JsonConvert.SerializeObject(item2, Formatting.Indented));
        }
    }

    public class SourceInfectionDate
    {
        public string Date { get; set; }
        public string FID { get; set; }
        public string CloseContact { get; set; }
        public string CruiseShip { get; set; }
        public string Interstate { get; set; }
        public string Overseas { get; set; }
        public string Pending { get; set; }
        public string Unknown { get; set; }
        public string TotalConfirmed { get; set; }
    }
}
