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
    public class CaseLGA
    {
        public static void Execute()
        {
            var client = new RestClient("https://services.arcgis.com/Qxcws3oU4ypcnx4H/arcgis/rest/services/confirmed_cases_by_LGA_view_layer/FeatureServer/0/query?f=json&resultOffset=0&resultRecordCount=8000&where=1=1&outFields=*&outSR=102100&resultType=standard&returnGeometry=false");
            client.Timeout = -1;
            client.UseNewtonsoftJson();
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            JObject des = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
            List<LGAStat> list = new List<LGAStat>();
            foreach (var it in des["features"])
            {
                var item = it.First().First();
                LGAStat stat = new LGAStat();
                var active = ((JValue)item["Active_Case"]).Value;
                if (active != null)
                {
                    stat.Active = active.ToString();
                }
                else
                {
                    stat.Active = "0";
                }
                var recov = ((JValue)item["PopUpLabel"]).Value;
                if (recov != null)
                {
                    stat.Recovered = recov.ToString();
                }
                else
                {
                    stat.Recovered = "0";
                }
                stat.LGANameFull = (string)((JValue)item["LGA_Name_Full"]).Value.ToString();
                stat.LGAName = (string)((JValue)item["LGA_NAME19"]).Value.ToString();
                list.Add(stat);
            }
            JsonObject item2 = new JsonObject();
            //item2.Add("date_updated", latestdate);
            item2.Add("data", list);
            File.WriteAllText("lga.json", JsonConvert.SerializeObject(item2, Formatting.Indented));
        }
    }

    public class LGAStat
    {
        public string LGAName { get; set; }
        public string LGANameFull { get; set; }
        public string Total { get; set; }
        public string Active { get; set; }
        public string Recovered { get; set; }
    }
}
