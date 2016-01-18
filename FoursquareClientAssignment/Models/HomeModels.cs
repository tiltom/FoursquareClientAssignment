using System.ComponentModel.DataAnnotations;

namespace FoursquareClientAssignment.Models
{
    /// <summary>
    ///     Model for search input
    /// </summary>
    public class IndexModel
    {
        /// <summary>
        ///     Query to find
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter at least 1 character.")]
        public string Query { get; set; }
    }

    /// <summary>
    ///     Venue model
    /// </summary>
    public class VenueSearchResultModel
    {
        /// <summary>
        ///     Unique Id (in Foursquare database)
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Name of the venue
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Number of checkins total
        /// </summary>
        [Display(Name = "Checkins total")]
        public long CheckinCount { get; set; }

        /// <summary>
        ///     Number of checkins now
        /// </summary>
        [Display(Name = "Checkins now")]
        public long HereNow { get; set; }
    }
}