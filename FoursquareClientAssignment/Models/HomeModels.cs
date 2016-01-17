using System.ComponentModel.DataAnnotations;

namespace FoursquareClientAssignment.Models
{
    public class IndexModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter at least 1 character.")]
        public string Query { get; set; }
    }

    public class VenueSearchResultModel
    {
        public string Id { get; set; }


        public string Name { get; set; }

        [Display(Name = "Count for lifetime")]
        public long CheckingCount { get; set; }

        [Display(Name = "Count now")]
        public long HereNow { get; set; }
    }
}