using System.ComponentModel.DataAnnotations;

namespace RestaurantRaterMVC.Models.Restaurant
{
    public class RestaurantEdit
    {
        public int Id {get; set;}
        [Required]
        [StringLength(100)]
        public string Name {get; set;}
        [Required]
        [StringLength(100)]
        public string Location {get; set;}
    }
}