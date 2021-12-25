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
    public class DetailsDate
    {
        public static void Execute()
        {
            var client = new RestClient("https://services.arcgis.com/Qxcws3oU4ypcnx4H/arcgis/rest/services/simple_dashboard_report_view_layer/FeatureServer/3/query?f=json&where=1=1&returnGeometry=false&spatialRel=esriSpatialRelIntersects&outFields=*&orderByFields=EditDate%20asc&resultOffset=0&resultType=standard&cacheHint=true");
            client.Timeout = -1;
            client.UseNewtonsoftJson();
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            JObject des = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
            List<DetailsDateOb> list = new List<DetailsDateOb>();
            foreach (var it in des["features"])
            {
                var item = it.First().First();
                DetailsDateOb stat = new DetailsDateOb();
                stat.Date = (string)((JValue)item["date"]).Value.ToString();
                long unixmil = long.Parse(stat.Date);
                var itdate = DateTimeOffset.FromUnixTimeMilliseconds(unixmil);
                stat.Date = itdate.Date.ToShortDateString();
                stat.ObjectId = (string)((JValue)item["OBJECTID"]).Value.ToString();
                stat.Confirmed = (string)((JValue)item["Confirmed"]).Value?.ToString();
                stat.Death = (string)((JValue)item["Death"]).Value?.ToString();
                stat.Hospitalised = (string)((JValue)item["Hospitalised"]).Value?.ToString();
                stat.TestNegative = (string)((JValue)item["Tested_nagative"]).Value?.ToString();
                stat.Recovered = (string)((JValue)item["Recovered"]).Value?.ToString();
                stat.ConfirmIncreaseDaily = (string)((JValue)item["Confirmed_increased_daily"]).Value?.ToString();
                stat.DeathIncrease = (string)((JValue)item["Death_increased"]).Value?.ToString();
                stat.HospitalIncrease = (string)((JValue)item["Hospitalised_increased"]).Value?.ToString();
                stat.DischargeIncrease = (string)((JValue)item["Discharge_increased"]).Value?.ToString();
                stat.TestNegativeIncrease = (string)((JValue)item["Tested_negative_increased"]).Value?.ToString();
                stat.GlobalId = (string)((JValue)item["GlobalID"]).Value?.ToString();
                stat.CreationDate = (string)((JValue)item["CreationDate"]).Value?.ToString();
                stat.Creator = (string)((JValue)item["Creator"]).Value?.ToString();
                stat.EditDate = (string)((JValue)item["EditDate"]).Value?.ToString();
                stat.Editor = (string)((JValue)item["Editor"]).Value?.ToString();
                stat.Discharge = (string)((JValue)item["Discharge"]).Value?.ToString();
                stat.LocallyTransmitted = (string)((JValue)item["Locally_transmitted"]).Value?.ToString();
                stat.SelfIsolated = (string)((JValue)item["Self_isolated"]).Value?.ToString();
                stat.SelfIsolatedIncrease = (string)((JValue)item["Self_isolated_increased"]).Value?.ToString();
                stat.ActiveCases = (string)((JValue)item["Active_cases"]).Value?.ToString();
                stat.ActiveIncrease = (string)((JValue)item["Active_increased"]).Value?.ToString();
                stat.ActiveIncreaseAbsolute = (string)((JValue)item["Active_increased_absolute"]).Value?.ToString();
                stat.EditDateText = (string)((JValue)item["EditDateText"]).Value?.ToString();
                stat.HospitalIncreaseAbsolute = (string)((JValue)item["Hospitalised_increased_absolute"]).Value?.ToString();
                stat.Historical = (string)((JValue)item["Historical"]).Value?.ToString();
                stat.HistoricalIncreaseDaily = (string)((JValue)item["Historical_increased_daily"]).Value?.ToString();
                stat.VaccineAdminstered = (string)((JValue)item["Vaccine_administered"]).Value?.ToString();
                stat.ReceiveBothDoses = (string)((JValue)item["Received_both_doses"]).Value?.ToString();
                stat.AgedCare = (string)((JValue)item["Aged_Care"]).Value?.ToString();
                stat.PrimaryCare = (string)((JValue)item["Primary_Care"]).Value?.ToString();
                stat.CumulativeTotal = (string)((JValue)item["Cumulative_Total"]).Value?.ToString();
                stat.VaccRate = (string)((JValue)item["Vaccination_Rate"]).Value?.ToString();
                stat.AstraZeneca_Past24 = (string)((JValue)item["AstraZeneca_Past24"]).Value?.ToString();
                stat.AstraZeneca_Total = (string)((JValue)item["AstraZeneca_Total"]).Value?.ToString();
                stat.Pfizer_Past24 = (string)((JValue)item["Pfizer_Past24"]).Value?.ToString();
                stat.Pfizer_Total = (string)((JValue)item["Pfizer_Total"]).Value?.ToString();
                stat.Dose1_Past24 = (string)((JValue)item["Dose1_Past24"]).Value?.ToString();
                stat.Dose2_Past24 = (string)((JValue)item["Dose2_Past24"]).Value?.ToString();
                stat.Dose1_Total = (string)((JValue)item["Dose1_Total"]).Value?.ToString();
                stat.Dose2_Total = (string)((JValue)item["Dose2_Total"]).Value?.ToString();
                stat.Rate_Dose1 = (string)((JValue)item["Rate_Dose1"]).Value?.ToString();
                stat.Rate_Dose2 = (string)((JValue)item["Rate_Dose2"]).Value?.ToString();
                stat.Total_Past24 = (string)((JValue)item["Total_Past24"]).Value?.ToString();
                stat.Data_Date_Text = (string)((JValue)item["Data_Date_Text"]).Value?.ToString();
                list.Add(stat);
            }
            JsonObject item2 = new JsonObject();
            //item2.Add("date_updated", latestdate);
            item2.Add("data", list);
            File.WriteAllText("details.json", JsonConvert.SerializeObject(item2, Formatting.Indented));
        }
    }

    public class DetailsDateOb
    {
        public string Date { get; set; }
        public string ObjectId { get; set; }
        public string Confirmed { get; set; }
        public string Death { get; set; }
        public string Hospitalised { get; set; }
        public string TestNegative { get; set; }
        public string Recovered { get; set; }
        public string ConfirmIncreaseDaily { get; set; }
        public string DeathIncrease { get; set; }
        public string HospitalIncrease { get; set; }
        public string DischargeIncrease { get; set; }
        public string TestNegativeIncrease { get; set; }
        public string GlobalId { get; set; }
        public string CreationDate { get; set; }
        public string Creator { get; set; }
        public string EditDate { get; set; }
        public string Editor { get; set; }
        public string Discharge { get; set; }
        public string LocallyTransmitted { get; set; }
        public string SelfIsolated { get; set; }
        public string SelfIsolatedIncrease { get; set; }
        public string ActiveCases { get; set; }
        public string ActiveIncrease { get; set; }
        public string ActiveIncreaseAbsolute { get; set; }
        public string EditDateText { get; set; }
        public string HospitalIncreaseAbsolute { get; set; }
        public string Historical { get; set; }
        public string HistoricalIncreaseDaily { get; set; }
        public string VaccineAdminstered { get; set; }
        public string ReceiveBothDoses { get; set; }
        public string AgedCare { get; set; }
        public string PrimaryCare { get; set; }
        public string CumulativeTotal { get; set; }
        public string VaccRate { get; set; }
        public string AstraZeneca_Past24 { get; set; }
        public string AstraZeneca_Total { get; set; }
        public string Pfizer_Past24 { get; set; }
        public string Pfizer_Total { get; set; }
        public string Dose1_Past24 { get; set; }
        public string Dose2_Past24 { get; set; }
        public string Dose1_Total { get; set; }
        public string Dose2_Total { get; set; }
        public string Rate_Dose1 { get; set; }
        public string Rate_Dose2 { get; set; }
        public string Total_Past24 { get; set; }
        public string Data_Date_Text { get; set; }
    }
}
