using System.ComponentModel.DataAnnotations.Schema;

namespace FoursquareClientAssignment.Entities
{
    public class QueryResult
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Query { get; set; }
        public int Count { get; set; }
    }
}