using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantRaterMVC.Data;
using RestaurantRaterMVC.Models.Restaurant;

namespace RestaurantRaterMVC.Controllers
{
    public class RestaurantController : Controller
    {
        private RestaurantRaterDbContext _context;
        //Direct Injection for Restaurant controller
        public RestaurantController(RestaurantRaterDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<RestaurantListItem> restaurants = await _context.Restaurants //sets a list of RestaurantListItem models by stepping into restaurant table in db context
                .Include(r => r.Ratings) //.Includes pulls the ratings even though they aren't part of the model as they are necessary to get average score
                .Select(r => new RestaurantListItem() 
                {
                    Id = r.Id,
                    Name = r.Name,
                    Score = r.Score,
                }).ToListAsync();

                return View(restaurants);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RestaurantCreate model)
        {
            //Error handling for invalid models
            if (!ModelState.IsValid)
            {
                ViewBag.errorMessage = ModelState.FirstOrDefault().Value.Errors.FirstOrDefault().ErrorMessage;
                return View(model);
            }
            
            //Builds new Restaurant using CreateRestaurant Model
            Restaurant restaurant = new Restaurant()
            {
                Name = model.Name,
                Location = model.Location,
            };

            //Adds restaurant to table, then saves changes
            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); //bring user back to Index view of Restaurants
        }
    }
}