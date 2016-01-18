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

        /// <summary>
        ///     Shows view with search input
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        ///     Shows results of search
        /// </summary>
        /// <param name="query">Searched phrase</param>
        /// <returns>View with list of venues</returns>
        [HttpPost]
        public ActionResult SearchResults(string query)
        {
            // if someone calls this action directly without search query (shouldn't happen)
            if (query.IsNullOrWhiteSpace())
            {
                this.AddError("Query cannot be empty, please type phrase.");
                return this.RedirectToAction("Index");
            }

            ViewBag.VenueQuery = query;

            // finds how many times the query has been searched
            this._homeService.SaveQueryResult(query);
            ViewBag.QueryResultCount = this._homeService.FindQueryResultCount(query);


            return this.View(this._homeService.GetVenuesListShowModel(query));
        }

        /// <summary>
        ///     Asynchronously load more venues to view
        /// </summary>
        /// <param name="size">Determines how many venues has been already loaded</param>
        /// <param name="venueQuery">Query used to find venues</param>
        /// <returns>Partial view with venues in Json format</returns>
        public JsonResult ShowMoreVenues(int size, string venueQuery)
        {
            // get more venues for the query
            var model = this._homeService.GetVenuesListShowModel(venueQuery, size);

            // get total count of venues for the query
            var modelCount = this._homeService.GetVenueListCount(venueQuery);

            ViewBag.VenueQuery = venueQuery;

            // if model is not empty (shouldn't be) then render it as a partial view and send it in Json format
            if (model.Any())
            {
                var modelString = this.RenderRazorViewToString("_ShowMoreVenues", model);
                return this.Json(new {ModelString = modelString, ModelCount = modelCount, VenueQuery = venueQuery});
            }
            return this.Json(model);
        }

        #region private

        /// <summary>
        ///     Add string to TempData in order to display it in layout as error message
        /// </summary>
        /// <param name="errorMessage">Error message we want to display</param>
        public void AddError(string errorMessage)
        {
            TempData[HomeConstant.Error] = errorMessage;
        }

        /// <summary>
        ///     Render view to string (used to "show more venues" functionality)
        /// </summary>
        /// <param name="viewName">Name of the view we want to render to string</param>
        /// <param name="model">Model passed to the view</param>
        /// <returns></returns>
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