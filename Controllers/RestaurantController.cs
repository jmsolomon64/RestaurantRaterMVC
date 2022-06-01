using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantRaterMVC.Data;
using RestaurantRaterMVC.Models.Restaurant;
using System.Threading;

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

        [ActionName("Details")]
        public async Task<IActionResult> Restaurant(int id)
        {
            Restaurant restaurant = _context.Restaurants
            .Include(r => r.Ratings)
            .FirstOrDefault(r => r.Id == id);

            if (restaurant == null)
            {
                return RedirectToAction(nameof(Index));
            }

            RestaurantDetail restaurantDetail = new RestaurantDetail()
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Location = restaurant.Location,
                Score = restaurant.Score
            };

            return View(restaurantDetail);
        }

        public async Task<IActionResult> Edit(int id)
        {
            Restaurant restaurant = await _context.Restaurants.FindAsync(id); //strores a restaurant from the dbset that matches Id


            //Redirects back to index if Id doesn't exist
            if (restaurant == null)
            {
                return RedirectToAction(nameof(Index));
            }

            //Sets restaurantEdit properties equal to Restaurant properties
            RestaurantEdit restaurantEdit = new RestaurantEdit()
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Location = restaurant.Location
            };

            return View(restaurantEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, RestaurantEdit model)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }

            //gets restaurant by id and if null returns to index
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);
            if(restaurant == null)
            {
                return RedirectToAction(nameof(Index));
            }

            //sets Restaurants properties equal to the new ones in RestaurantEdit
            restaurant.Name = model.Name;
            restaurant.Location = model.Location;
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Details", new {id = restaurant.Id});
        }
    }
}