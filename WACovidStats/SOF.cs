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
    public class SOF
    {
        public static void Execute()
        {
            var client = new RestClient("https://services.arcgis.com/Qxcws3oU4ypcnx4H/arcgis/rest/services/Source_of_infection_view_layer/FeatureServer/0/query?f=json&where=1%3D1&returnGeometry=false&spatialRel=esriSpatialRelIntersects&outFields=*&orderByFields=Display_order%20asc&resultOffset=0&resultRecordCount=25&resultType=standard&cacheHint=true");
            client.Timeout = -1;
            client.UseNewtonsoftJson();
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            JObject des = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
            List<SourceInfection> list = new List<SourceInfection>();
            foreach (var it in des["features"])
            {
                var item = it.First().First();
                SourceInfection stat = new SourceInfection();
                stat.Name = (string)((JValue)item["Name"]).Value.ToString();
                stat.Number = (string)((JValue)item["Number"]).Value.ToString();
                stat.Active = (string)((JValue)item["Active"]).Value.ToString();
                stat.NumberLast7Days = (string)((JValue)item["NumberLast7Days"]).Value.ToString();
                stat.NumberLast24Hours = (string)((JValue)item["NumberLast24Hours"]).Value.ToString();
                list.Add(stat);
            }
            JsonObject item2 = new JsonObject();
            //item2.Add("date_updated", latestdate);
            item2.Add("data", list);
            File.WriteAllText("docs/sourceofinfection.json", JsonConvert.SerializeObject(item2, Formatting.Indented));
        }
    }

    public class SourceInfection
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public string Active { get; set; }
        public string NumberLast7Days { get; set; }
        public string NumberLast24Hours { get; set; }
    }
}
