using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantRaterMVC.Data;
using RestaurantRaterMVC.Models.Rating;

namespace RestaurantRaterMVC.Controllers
{
    public class RatingController : Controller
    {
        private RestaurantRaterDbContext _context;
        
        public RatingController(RestaurantRaterDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<RatingListItem> ratings = await _context.Ratings
            .Select(r => new RatingListItem()
            {
                Id = r.Id,
                RestaurantName = r.Restaurant.Name,
                Score = r.Score
            }).ToListAsync();

            return View(ratings);
        }

        public async Task<IActionResult> Restaurant(int id)
        {
            IEnumerable<RatingListItem> ratings = await _context.Ratings
            .Where(r => r.RestaurantId == id)
            .Select(r => new RatingListItem()
            {
                Id = r.Id,
                RestaurantName = r.Restaurant.Name,
                Score = r.Score
            }).ToListAsync();

            Restaurant restaurant = await _context.Restaurants.FindAsync(id);
            ViewBag.RestaurantName = restaurant.Name;

            return View(ratings);
        }

        public async Task<IActionResult> Create()
        {
            IEnumerable<SelectListItem> restaurantOptions = await _context.Restaurants.Select(r => new SelectListItem()
            {
                Text = r.Name,
                Value = r.Id.ToString()
            }).ToListAsync();

            RatingCreate model = new RatingCreate();
            model.RestaurantOptions = restaurantOptions;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RatingCreate model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Rating rating = new Rating()
            {
                RestaurantId = model.RestaurantId,
                Score = model.Score
            };

            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            //Get list of ratings that share restaurant Id
            //Set them as models
            //return them to the view
            Rating rating = await _context.Ratings.FirstOrDefaultAsync(r => r.Id == id);
            if (rating == null)
            {
                return RedirectToAction(nameof(Index));
            }

            RatingListItem ratingListItem = new RatingListItem()
            {
                Id = rating.Id,
                RestaurantName = rating.Restaurant.Name,
                Score = rating.Score
            };

            return View(ratingListItem);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, RatingListItem model)
        {
            Rating rating = await _context.Ratings.FirstOrDefaultAsync(r => r.Id ==id);
            if (rating == null)
            {
                return RedirectToAction(nameof(Index));
            }

            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}