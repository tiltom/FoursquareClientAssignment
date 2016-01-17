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
        private static readonly string _clientId = WebConfigurationManager.AppSettings[HomeConstant.ClientId];
        private static readonly string _clientSecret = WebConfigurationManager.AppSettings[HomeConstant.ClientSecret];

        private readonly FoursquareClientAssignmentContext db = new FoursquareClientAssignmentContext();

        public QueryResult FindQueryResult(string query)
        {
            return this.db.QueryResults.FirstOrDefault(x => x.Query == query);
        }

        public void SaveQueryResult(string query)
        {
            var queryResult = this.FindQueryResult(query);

            if (queryResult == null)
            {
                this.db.QueryResults.Add(new QueryResult {Count = 1, Query = query});
                this.db.SaveChanges();
            }
            else
            {
                queryResult.Count += 1;
                this.db.SaveChanges();
            }
        }

        public int NumberOfQueryResult(string query)
        {
            return this.FindQueryResult(query).Count;
        }

        public int GetVenueListCount(string venueQuery)
        {
            var sharpSquare = new SharpSquare(_clientId, _clientSecret);

            return sharpSquare.SearchVenues(new Dictionary<string, string>
            {
                {"near", HomeConstant.NearCity},
                {"query", venueQuery}
            }).Count;
        }

        public List<VenueSearchResultModel> GetVenuesListShowModel(string venueQuery, int skipSize = 0)
        {
            var sharpSquare = new SharpSquare(_clientId, _clientSecret);

            var venues = sharpSquare.SearchVenues(new Dictionary<string, string>
            {
                {"near", HomeConstant.NearCity},
                {"query", venueQuery}
            }).Skip(skipSize).Take(HomeConstant.PageSize);

            var venueShowList = new List<VenueSearchResultModel>();

            foreach (var venue in venues)
            {
                venueShowList.Add(new VenueSearchResultModel
                {
                    HereNow = venue.hereNow.count,
                    Name = venue.name,
                    CheckingCount = venue.stats.checkinsCount,
                    Id = venue.id
                });
            }

            return venueShowList;
        }
    }
}