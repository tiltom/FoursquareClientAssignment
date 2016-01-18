using System.ComponentModel.DataAnnotations.Schema;

namespace FoursquareClientAssignment.Entities
{
    /// <summary>
    ///     Entity used to save and retrieve number of times the query has been searched
    /// </summary>
    public class QueryResult
    {
        /// <summary>
        ///     Unique Id of entity
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        ///     Searched query
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        ///     Count of how many times it has been searched
        /// </summary>
        public int Count { get; set; }
    }
}