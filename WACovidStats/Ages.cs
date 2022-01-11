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
    public class Ages
    {
        public static void Execute()
        {
            var client = new RestClient("https://services.arcgis.com/Qxcws3oU4ypcnx4H/arcgis/rest/services/Age_sex_total_COVID19_Chart_view_layer/FeatureServer/0/query?f=json&where=1=1&returnGeometry=true&spatialRel=esriSpatialRelIntersects&outFields=*&resultOffset=0&resultRecordCount=50&resultType=standard&cacheHint=true");
            client.Timeout = -1;
            client.UseNewtonsoftJson();
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            JObject des = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
            List<AgesOb> list = new List<AgesOb>();
            foreach (var it in des["features"])
            {
                var item = it.First().First();
                AgesOb stat = new AgesOb();
                stat.FID = (string)((JValue)item["FID"]).Value.ToString();
                stat.Age_Group = (string)((JValue)item["Age_Group"]).Value.ToString();
                stat.Male = (string)((JValue)item["Male"]).Value.ToString();
                stat.Female = (string)((JValue)item["Female"]).Value.ToString();
                stat.Total = (string)((JValue)item["Total"]).Value.ToString();
                list.Add(stat);
            }
            JsonObject item2 = new JsonObject();
            //item2.Add("date_updated", latestdate);
            item2.Add("data", list);
            File.WriteAllText("agegroups.json", JsonConvert.SerializeObject(item2, Formatting.Indented));
        }
    }

    public class AgesOb
    {
        public string FID { get; set; }
        public string Age_Group { get; set; }
        public string Male { get; set; }
        public string Female { get; set; }
        public string Total { get; set; }
    }
}
