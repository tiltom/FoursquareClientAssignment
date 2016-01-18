using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using FoursquareClientAssignment.Constants;
using FoursquareClientAssignment.Database;
using FoursquareClientAssignment.Entities;
using FoursquareClientAssignment.Models;
using FourSquare.SharpSquare.Core;

namespace FoursquareClientAssignment.Business_logic
{
    public class HomeService
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly FoursquareClientAssignmentContext _db;
        private readonly SharpSquare _sharpSquare;

        public HomeService()
        {
            this._clientId = WebConfigurationManager.AppSettings[HomeConstant.ClientId];
            this._clientSecret = WebConfigurationManager.AppSettings[HomeConstant.ClientSecret];
            this._sharpSquare = new SharpSquare(this._clientId, this._clientSecret);
            this._db = new FoursquareClientAssignmentContext();
        }

        /// <summary>
        ///     Finds QueryResult object from database by query
        /// </summary>
        /// <param name="query">Searched phrase</param>
        /// <returns>QueryResult object or null(if it doesn't exist)</returns>
        public QueryResult FindQueryResult(string query)
        {
            return this._db.QueryResults.FirstOrDefault(x => x.Query == query);
        }

        /// <summary>
        ///     Save query to database or update query if already exists
        /// </summary>
        /// <param name="query">Searched phrase</param>
        public void SaveQueryResult(string query)
        {
            var queryResult = this.FindQueryResult(query);

            // if query is not in database then create it and set Count to 1
            if (queryResult == null)
            {
                this._db.QueryResults.Add(new QueryResult {Count = 1, Query = query});
                this._db.SaveChanges();
            }
            // if query is already present in database then increment Count
            else
            {
                queryResult.Count += 1;
                this._db.SaveChanges();
            }
        }

        /// <summary>
        ///     Finds how many times has been query searched.
        /// </summary>
        /// <param name="query">Searched phrase</param>
        /// <returns>Number of how many times has been query searched</returns>
        public int FindQueryResultCount(string query)
        {
            return this.FindQueryResult(query).Count;
        }

        /// <summary>
        ///     Finds how many venues are returned for the query.
        /// </summary>
        /// <param name="venueQuery">Searched phrase</param>
        /// <returns>Count of returned venues</returns>
        public int GetVenueListCount(string venueQuery)
        {
            return this._sharpSquare.SearchVenues(new Dictionary<string, string>
            {
                {"near", HomeConstant.NearCity},
                {"query", venueQuery}
            }).Count;
        }

        /// <summary>
        ///     Finds venues by entered query.
        /// </summary>
        /// <param name="venueQuery">Searched phrase</param>
        /// <param name="skipSize">
        ///     Optional parameter(it is used in Ajax loading) which determines how many search results of
        ///     venues should we skip in loading
        /// </param>
        /// <returns>Collection of venues in VenueSearchResultModel</returns>
        public List<VenueSearchResultModel> GetVenuesListShowModel(string venueQuery, int skipSize = 0)
        {
            // find venues according to the query
            var venues = this._sharpSquare.SearchVenues(new Dictionary<string, string>
            {
                {"near", HomeConstant.NearCity},
                {"query", venueQuery}
            }).Skip(skipSize).Take(HomeConstant.PageSize);

            var venueShowList = new List<VenueSearchResultModel>();

            // convert list of venues to VenuesListShowModel
            foreach (var venue in venues)
            {
                venueShowList.Add(new VenueSearchResultModel
                {
                    HereNow = venue.hereNow.count,
                    Name = venue.name,
                    CheckinCount = venue.stats.checkinsCount,
                    Id = venue.id
                });
            }

            return venueShowList;
        }
    }
}