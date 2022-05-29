using System.ComponentModel.DataAnnotations;

namespace RestaurantRaterMVC.Models.Restaurant
{
    public class RestaurantCreate
    {
        [Required]
        [MaxLength(100)]
        public string Name {get; set;}
        [Required]
        [MaxLength(100)]
        public string Location {get; set;}
    }
}