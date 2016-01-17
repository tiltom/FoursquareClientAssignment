using System.Collections.Generic;
using System.Data.Entity;
using FoursquareClientAssignment.Entities;

namespace FoursquareClientAssignment.Database
{
    public class FourSquareClientAssignmentInitializer :
        DropCreateDatabaseIfModelChanges<FoursquareClientAssignmentContext>
    {
        protected override void Seed(FoursquareClientAssignmentContext context)
        {
            var queryResults = new List<QueryResult>
            {
                new QueryResult {Count = 0, Query = "Test"},
                new QueryResult {Count = 0, Query = "Test2"}
            };

            queryResults.ForEach(s => context.QueryResults.Add(s));
            context.SaveChanges();
        }
    }
}