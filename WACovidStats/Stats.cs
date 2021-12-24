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
    public class Stats
    {
        public static void Execute()
        {
            var client = new RestClient("https://services.arcgis.com/Qxcws3oU4ypcnx4H/arcgis/rest/services/COVID19_Dashboard_Chart_ViewLayer/FeatureServer/0/query?f=json&where=date%3Etimestamp%20%272020-02-16%2015%3A59%3A59%27&returnGeometry=false&spatialRel=esriSpatialRelIntersects&outFields=*&orderByFields=date%20asc&resultOffset=0&resultRecordCount=32000&resultType=standard&cacheHint=true");
            client.Timeout = -1;
            client.UseNewtonsoftJson();
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            JObject des = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
            List<StatsOb> list = new List<StatsOb>();
            foreach (var it in des["features"])
            {
                var item = it.First().First();
                StatsOb stat = new StatsOb();
                stat.Date = (string)((JValue)item["date"]).Value.ToString();
                long unixmil = long.Parse(stat.Date);
                var itdate = DateTimeOffset.FromUnixTimeMilliseconds(unixmil);
                stat.Date = itdate.Date.ToShortDateString();
                stat.new_cases = (string)((JValue)item["new_cases"]).Value.ToString();
                stat.total_cases = (string)((JValue)item["total_cases"]).Value.ToString();
                stat.total_recovered = (string)((JValue)item["total_recovered"]).Value.ToString();
                stat.total_death = (string)((JValue)item["total_death"]).Value.ToString();
                stat.existing = (string)((JValue)item["existing_cases"]).Value.ToString();
                //stat.total_ruledout = (string)((JValue)item["total_ruledout"]).Value.ToString();
                //stat.hospitalized = (string)((JValue)item["hospitalized"]).Value.ToString();
                list.Add(stat);
            }
            JsonObject item2 = new JsonObject();
            //item2.Add("date_updated", latestdate);
            item2.Add("data", list);
            File.WriteAllText("totals.json", JsonConvert.SerializeObject(item2, Formatting.Indented));
        }
    }

    public class StatsOb
    {
        public string Date { get; set; }
        public string new_cases { get; set; }
        public string total_cases { get; set; }
        public string total_recovered { get; set; }
        public string total_death { get; set; }
        public string existing { get; set; }
        public int? total_ruledout { get; set; }
        public int? hospitalized { get; set; }
    }
}
