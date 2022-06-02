using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RestaurantRaterMVC.Models.Rating
{
    public class RatingCreate
    {
        [Required]
        [Display(Name = "Restaurant")]
        public int RestaurantId {get; set;}
        [Required]
        [Range(1, 10)]
        public double Score {get; set;}

        public IEnumerable<SelectListItem> RestaurantOptions {get; set;} = new List<SelectListItem>();
    }
}