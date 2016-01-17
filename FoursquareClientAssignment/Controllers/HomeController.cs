using System.IO;
using System.Linq;
using System.Web.Mvc;
using FoursquareClientAssignment.Business_logic;
using FoursquareClientAssignment.Constants;
using Microsoft.Ajax.Utilities;

namespace FoursquareClientAssignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly HomeService _homeService = new HomeService();

        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult SearchResults(string query)
        {
            if (query.IsNullOrWhiteSpace())
            {
                this.AddError("Query cannot be empty, please type phrase.");
                return this.RedirectToAction("Index");
            }

            ViewBag.VenueQuery = query;

            if (!query.IsNullOrWhiteSpace())
            {
                this._homeService.SaveQueryResult(query);
                ViewBag.QueryResultCount = this._homeService.NumberOfQueryResult(query);
            }
            else
                ViewBag.QueryResultCount = 0;

            return this.View(this._homeService.GetVenuesListShowModel(query));
        }

        public JsonResult ShowMoreVenues(int size, string venueQuery)
        {
            var model = this._homeService.GetVenuesListShowModel(venueQuery, size);

            var modelCount = this._homeService.GetVenueListCount(venueQuery);

            ViewBag.VenueQuery = venueQuery;

            if (model.Any())
            {
                var modelString = this.RenderRazorViewToString("_ShowMoreVenues", model);
                return this.Json(new {ModelString = modelString, ModelCount = modelCount, VenueQuery = venueQuery});
            }
            return this.Json(model);
        }

        #region private

        public void AddError(string errorMessage)
        {
            TempData[HomeConstant.Error] = errorMessage;
        }

        private string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                    viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                    ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        #endregion
    }
}