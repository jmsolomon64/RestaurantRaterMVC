using System.ComponentModel.DataAnnotations;

namespace RestaurantRaterMVC.Models.Rating
{
    public class RatingListItem
    {
        public int Id {get; set;}
        [Display(Name = "Restaurant")]
        public string RestaurantName {get; set;}
        [Display(Name = "Rating")]
        public double Score {get; set;}
    }
}