using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YemekSepeti.DTO;
using YemekSepeti.Models;

namespace YemekSepeti.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantReviewsController : Controller
    {
        //Dependency injectionla veritabani baglama islemleri
        private readonly YemekSepetContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;
        public RestaurantReviewsController(YemekSepetContext context, UserManager<IdentityUser<int>> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        //tum restoranlarin yorumlarinin datalarini alma
        [HttpGet]
        public async Task<IActionResult> GetAllRestaurantReviews()
        {
            List<RestaurantReview> restaurantReviews = await _context.RestaurantReviews.ToListAsync();
            return Ok(restaurantReviews);
        }
        //spesifik restoranin yorumlarini alma
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRestaurantReviewsById(int? id)
        {
            RestaurantReview? exactRestaurantReview = await _context.RestaurantReviews.FirstOrDefaultAsync(c => c.Id == id);
            if (exactRestaurantReview == null || id == null)
            {
                return BadRequest("There is no Restaurant Review with given id");
            }
            else
            {
                return Ok(exactRestaurantReview);
            }
        }
        [HttpPost]
        [Authorize(Roles = "Customer")]
        //restorana yorum ekleme
        public async Task<IActionResult> AddRestaurantReview(RestaurantReviewDTO entity)
         {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser.Id == entity.CustomerId)
            {
                if (entity.Comment.Trim() == "")
                {
                    return BadRequest("Can't comment blank");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Customer exactCustomer = await _context.Customers.FirstOrDefaultAsync(r => r.Email == currentUser.Email);
                if (exactCustomer == null)
                {
                    return BadRequest("There is no customer with given id");
                }
                Restaurant exactRestaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.Id == entity.RestaurantId);
                if (exactRestaurant == null)
                {
                    return BadRequest("There is no restaurant with given id");
                }
                RestaurantReview newRestaurantReview = new RestaurantReview()
                {
                    Rating = entity.Rating,
                    Comment =entity.Comment,
                    ReviewDate = DateTime.UtcNow,
                    CustomerId = entity.CustomerId,
                    RestaurantId = entity.RestaurantId,
                    Customer = exactCustomer,
                    Restaurant = exactRestaurant
                };
                _context.RestaurantReviews.Add(newRestaurantReview);
                await _context.SaveChangesAsync();
                return Ok("The new restaurant review has been added");
            }
            else
            {
                return Forbid("Only the customer can add this information.");
            }

            }
        [HttpPost("{id}")]
        [Authorize(Roles = "Customer")]
        //restoran yorumunu guncelleme
        public async Task<IActionResult> UpdateRestaurantReview(int? id, RestaurantReview entity)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id == entity.CustomerId)
            {
                if (id != entity.Id)
                {
                    return BadRequest("Ids are not matching");
                }
                RestaurantReview? exactRestaurantReview = await _context.RestaurantReviews.FirstOrDefaultAsync(m => m.Id == id);
                if (exactRestaurantReview == null)
                {
                    return BadRequest("There is no meal with given id");
                }
                exactRestaurantReview.Rating = entity.Rating;
                exactRestaurantReview.Comment = entity.Comment;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return NotFound();
                }
                return Ok("The review has been successfully updated");
            }
            else
            {
                return Forbid("Only the customer can add this information.");
            }

        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin,Customer")]
        //restoran yorumunu silme
        public async Task<IActionResult> DeleteRestaurantReview(int? id)
        {
            var currentUser = await _userManager.GetUserAsync(User);


            RestaurantReview exactRestaurantReview = await _context.RestaurantReviews.FirstOrDefaultAsync(c => c.Id == id);
            var exactCustomer = _context.Customers.FirstOrDefault(c => c.Id == exactRestaurantReview.CustomerId);

            if (exactRestaurantReview == null || id == null)
            {
                return BadRequest("We couldt find any review with that id");
            }
            if (User.IsInRole("SuperAdmin") || currentUser.Email == exactCustomer.Email)
            {
                _context.RestaurantReviews.Remove(exactRestaurantReview);
                await _context.SaveChangesAsync();
                return Ok("The review has been deleted successfully");
            }
            else
            {
                return Forbid("Only the SuperAdmin or the customer themselves can delete this information.");
            }

        }
        [HttpGet("{restaurantId}/reviewsByRestaurant")]
   
        //restorana gore yorumlari getirme
        public async Task<IActionResult> GetReviewsByRestaurant(int restaurantId)
        {
            IdentityUser<int> restaurant = await _userManager.FindByIdAsync(restaurantId.ToString());

            Restaurant? exactRestaurant = await _context.Restaurants
                .FirstOrDefaultAsync(r => r.Id == restaurantId
                );

            if (exactRestaurant == null)
            {
                return BadRequest("There is no restaurant with the given ID");
            }

            var reviews = await _context.RestaurantReviews
            .FromSqlRaw("EXEC up_RestoranYorumlariniAl @RestaurantId = {0}", exactRestaurant.Id)
            .ToListAsync();
            //List<RestaurantReview> reviews = await _context.RestaurantReviews
            //    .Where(r => r.RestaurantId == exactRestaurant.Id)
            //    .Include(r => r.Customer) 
            //    .ToListAsync();


            var reviews1 = reviews
                .Select(review => new ReviewDTO
                {
                    Id = review.Id,
                    Comment = review.Comment,
                    Rating = review.Rating,
                    ReviewDate = review.ReviewDate,
                    CustomerName = _context.Customers.FirstOrDefault(c=> c.Id == review.CustomerId).Name, 
                    CustomerId = review.CustomerId
                })
                .ToList();

            return Ok(reviews1); 
        }

        [HttpGet("{restaurantId}/reviewsByRestaurant-inAdmin")]
        [Authorize(Roles = "SuperAdmin,Restaurant,Customer")]
        public async Task<IActionResult> GetReviewsByRestaurantAdmin(int restaurantId)
        {
            IdentityUser<int> restaurant = await _userManager.FindByIdAsync(restaurantId.ToString());

            Restaurant? exactRestaurant = await _context.Restaurants
                .Include(r => r.RestaurantReviews)
                .FirstOrDefaultAsync(r => r.Email == restaurant.Email
                );

            if (exactRestaurant == null)
            {
                return BadRequest("There is no restaurant with the given ID");
            }

            var reviews = await _context.RestaurantReviews
            .FromSqlRaw("EXEC up_RestoranYorumlariniAl @RestaurantId = {0}", exactRestaurant.Id)
            .ToListAsync();


            var reviews1 = reviews
                .Select(review => new ReviewDTO
                {
                    Id = review.Id,
                    Comment = review.Comment,
                    Rating = review.Rating,
                    ReviewDate = review.ReviewDate,
                    CustomerName = _context.Customers.FirstOrDefault(c => c.Id == review.CustomerId).Name,
                    CustomerId = review.CustomerId
                })
                .ToList();

            return Ok(reviews1); 
        }

        
    }
}
