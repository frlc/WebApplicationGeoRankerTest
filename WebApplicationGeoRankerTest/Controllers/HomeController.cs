using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationGeoRankerTest.Models;

namespace WebApplicationGeoRankerTest.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Models.ReportViewModel reportViewModel)
        {
            string apiKey = "c97d2240aab9e1492d3f664b20954efc";
            List<ReportViewModel> listReportViewModel = new List<ReportViewModel>();
            reportViewModel.type = "ranktracker";
            
            if (!ModelState.IsValid)
            {
                return View(reportViewModel.url);
            }

            if (reportViewModel.url != null && reportViewModel.searchEngines != null && reportViewModel.keywords != null && reportViewModel.email != null && reportViewModel.countries != null)
            {
                using (var geoRanker = new GeoRankerViewModel(reportViewModel.email, apiKey))
                {

                    reportViewModel.session = geoRanker.GeoRankerGetSession(geoRanker.Email, geoRanker.ApiKey);


                    var resultReport = geoRanker.GeoRankerNewReport(reportViewModel);

                    listReportViewModel = geoRanker.GeoRankerGetListReport(reportViewModel.email, reportViewModel.session);

                    ViewBag.Reports = listReportViewModel;

                    ModelState.Clear();

                }
            }
            

            return View();
        }

        
    }
}