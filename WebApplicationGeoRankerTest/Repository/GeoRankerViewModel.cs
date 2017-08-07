using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace WebApplicationGeoRankerTest.Models
{
    public class GeoRankerViewModel : IDisposable
    {

        public string Email { get; set; }
        public string ApiKey { get; set; }
        public string Session { get; set; }

        string apiUrl = "https://api.georanker.com/v1/";


        public GeoRankerViewModel(string email, string apiKey)
        {
            this.Email = email;
            this.ApiKey = apiKey;

        }

        public string GeoRankerGetSession(string email, string apiKey)
        {
            string resultSession = "";
            var urlPartial = "api/login.json?email=" + email + "&apikey=" + apiKey;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetStringAsync(urlPartial).Result;
                    if (response.Length > 0)
                    {
                        JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        dynamic result = serializer.DeserializeObject(response);

                        foreach (KeyValuePair<string, object> entry in result)
                        {
                            if (entry.Key == "session")
                            {
                                resultSession = entry.Value.ToString();
                            }

                        }
                    }

                }
            }
            catch (Exception e)
            {

                throw e;
            }


            return resultSession;
        }


        public ReportViewModel GeoRankerNewReport(ReportViewModel reportViewModel)
        {
            
            ReportViewModel resultReport = new ReportViewModel();
            var urlPartial = "report/new.json?email=" + reportViewModel.email + "&session=" + reportViewModel.session;
            

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string jsonObj = JsonConvert.SerializeObject(reportViewModel);
                    HttpResponseMessage response = client.PostAsJsonAsync(urlPartial, jsonObj).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        resultReport = response.Content.ReadAsAsync<ReportViewModel>().Result;
                    }

                }
            }
            catch (Exception e)
            {

                throw e;
            }


            return resultReport;

        }


        public List<ReportViewModel> GeoRankerGetListReport(string email, string session)
        {
            List<ReportViewModel> listReportViewModel = new List<ReportViewModel>();
            var urlPartial = "report/list.json?email=" + email + "&session=" + session;
           

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetStringAsync(urlPartial).Result;
                    


                    if (response.Length > 0)
                    {
                        List<string> listKeywords = null;
                        List<string> listCountries = null;
                        List<string> listEngines = null;
                        List<string> listUrls = null;

                        var jsonReport = JObject.Parse(response);
                        var items = jsonReport["items"];

                        foreach (var item in items)
                        {
                            ReportViewModel report = new ReportViewModel();

                            report.type = item["type"].ToString();


                            foreach (var keyword in item["keywords"])
                            {
                                listKeywords = new List<string>();
                                listKeywords.Add(keyword.ToString());                                
                            }

                            foreach (var country in item["countries"])
                            {   
                                listCountries = new List<string>();
                                listCountries.Add(country.ToString());
                            }

                            foreach (var engine in item["searchengines"])
                            {
                                listEngines = new List<string>();
                                listEngines.Add(engine.ToString());
                            }

                            foreach (var url in item["urls"])
                            {
                                listUrls = new List<string>();
                                listUrls.Add(url["url"].ToString());
                            }

                            report.keywords = string.Join(",", listKeywords);
                            report.searchEngines = string.Join(",", listEngines);
                            report.countries = string.Join(",", listCountries);
                            report.url = string.Join(",", listUrls);


                            listReportViewModel.Add(report);
                        }

                    }

                }
            }
            catch (Exception e)
            {

                throw e;
            }


            return listReportViewModel;
        }


        


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}