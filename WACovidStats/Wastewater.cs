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
    public class Wastewater
    {
        public static void Execute()
        {
            var client = new RestClient("https://services.arcgis.com/Qxcws3oU4ypcnx4H/arcgis/rest/services/WWTPs_VL/FeatureServer/0/query?f=json&where=1=1&returnGeometry=false&spatialRel=esriSpatialRelIntersects&outFields=*&orderByFields=WWTP%20asc&resultOffset=0&resultType=standard&cacheHint=true");
            client.Timeout = -1;
            client.UseNewtonsoftJson();
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            JObject des = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
            List<WastewaterOb> list = new List<WastewaterOb>();
            foreach (var it in des["features"])
            {
                var item = it.First().First();
                WastewaterOb stat = new WastewaterOb();
                stat.FID = (string)((JValue)item["FID"]).Value?.ToString();
                stat.WWTP = (string)((JValue)item["WWTP"]).Value?.ToString();
                stat.Latest_Res = (string)((JValue)item["Latest_Res"]).Value?.ToString();
                stat.Date_Colle = (string)((JValue)item["Date_Colle"]).Value?.ToString();
                stat.GlobalID = (string)((JValue)item["GlobalID"]).Value?.ToString();
                stat.Metro_Region = (string)((JValue)item["Metro_Region"]).Value?.ToString();
                stat.Result_Image = (string)((JValue)item["Result_Image"]).Value?.ToString();
                stat.Symbol = (string)((JValue)item["Symbol"]).Value?.ToString();
                stat.CatchmentType = (string)((JValue)item["CatchmentType"]).Value?.ToString();
                stat.Latest_Results_Capitalize = (string)((JValue)item["Latest_Results_Capitalize"]).Value?.ToString();
                list.Add(stat);
            }
            JsonObject item2 = new JsonObject();
            //item2.Add("date_updated", latestdate);
            item2.Add("data", list);
            File.WriteAllText("wastewater.json", JsonConvert.SerializeObject(item2, Formatting.Indented));
        }
    }

    public class WastewaterOb
    {
        public string FID { get; set; }
        public string WWTP { get; set; }
        public string Latest_Res { get; set; }
        public string Date_Colle { get; set; }
        public string GlobalID { get; set; }
        public string Metro_Region { get; set; }
        public string Result_Image { get; set; }
        public string Symbol { get; set; }
        public string CatchmentType { get; set; }
        public string Latest_Results_Capitalize { get; set; }
    }
}
