using System.ComponentModel.DataAnnotations;

namespace RestaurantRaterMVC.Data
{
    public class Restaurant
    {
        [Key]
        public int Id {get; set;}
        [Required]
        [MaxLength(100)]
        public string Name {get; set;}
        [Required]
        [MaxLength(100)]
        public string Location {get; set;}

        public double Score 
        {
            get
            {
                return Ratings.Count > 0 ? Ratings.Select(r => r.Score).Sum() / Ratings.Count : 0; //divides sum by count of ratings to return an average rating
            }
        }
        public virtual List<Rating> Ratings {get; set;} = new List<Rating>(); //generates list of ratings for a restaurant
    }
}