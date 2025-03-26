using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Text.RegularExpressions;
using YemekSepeti.DTO;
using YemekSepeti.Functions;
using YemekSepeti.Models;

namespace YemekSepeti.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //Dependency Injectionla veritabani contextine baglanma islemleri
    public class RestaurantController : Controller
    {
        private readonly YemekSepetContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;
        public RestaurantController(YemekSepetContext context, UserManager<IdentityUser<int>> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        //tum restoranlarin datasini alma
        public async Task<IActionResult> GetAllRestaurants()
        {
            List<Restaurant> restaurants = await _context.Restaurants.ToListAsync();
            return Ok(restaurants);
        }
        [HttpGet("{id}")]
        //spesifik restoran datasini alma
        public async Task<IActionResult> GetRestaurantById(int id)
        {
            var exactRestaurant = await _userManager.FindByIdAsync(id.ToString());
            Restaurant restaurant = await _context.Restaurants.FirstOrDefaultAsync(restaurant => restaurant.Email == exactRestaurant.Email);
            if (id == null || restaurant == null)
            {
                return BadRequest();
            }
            return Ok(restaurant);
        }
        [HttpGet("{restaurantId}/getAverageMealPrice")]
        public async Task<object> GetAverageMealPriceAsync(int restaurantId)
        {
            decimal averagePrice = 0;
            decimal totalRevenue = 0;
            var email = _userManager.Users.FirstOrDefault(r => r.Id == restaurantId).Email;
            var exactRestaurant = _context.Restaurants.FirstOrDefault(r => r.Email == email);
            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT dbo.YiyecekFiyatOrtalamasiniAl(@RestaurantId)";
                    command.Parameters.Add(new SqlParameter("@RestaurantId", exactRestaurant.Id));

                    var result = await command.ExecuteScalarAsync();
                    averagePrice = Convert.ToDecimal(result);
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT dbo.TotalCiroyuHesapla(@RestaurantId)";
                    command.Parameters.Add(new SqlParameter("@RestaurantId", exactRestaurant.Id));

                    var result = await command.ExecuteScalarAsync();
                    if (result != new { })
                    {
                        totalRevenue = 0;
                    }
                    else
                    {
                    totalRevenue = Convert.ToDecimal(result);
                    }
                }
            }

            return new { averagePrice = averagePrice, totalRevenue = totalRevenue };
        }

        [HttpGet("{id}/meals")]
        //restoranin yemeklerinin datalarini alma
        public async Task<IActionResult> GetRestaurantMeals(int id)
        {
            var exactRestaurant = await _userManager.FindByIdAsync(id.ToString());
            var restaurant = await _context.Restaurants
                                    .Include(r => r.Meals)  
                                    .FirstOrDefaultAsync(r => r.Email == exactRestaurant.Email);

            if (restaurant == null)
            {
                return NotFound();
            }
            Console.WriteLine("Meals count: " + restaurant.Meals.Count);
            return Ok(restaurant.Meals.ToList());
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin, Restaurant")]
        public async Task<IActionResult> UpdateRestaurant(int id, EditRestaurantDTO entity)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var restaurant = await _userManager.FindByIdAsync(id.ToString());

            if (User.IsInRole("SuperAdmin") || currentUser.Id == id)
            {
                if (id != currentUser.Id)
                {
                    return BadRequest();
                }


                Restaurant exactRestaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.Email == restaurant.Email);
                if (exactRestaurant == null)
                {
                    return BadRequest("There is no restaurant with given id");
                }

                if (HelperFunctions.Check(entity) == false)
                {
                    return BadRequest("Enter proper restaurant details");
                }
                exactRestaurant.Name = entity.Name;
                exactRestaurant.Address = entity.Address;
                exactRestaurant.PhoneNumber = entity.PhoneNumber;
                exactRestaurant.WorkingHours = entity.WorkingHours;
                exactRestaurant.Image = entity.Image;


                currentUser.UserName = entity.Name;


                var tokenPhone = await _userManager.GenerateChangePhoneNumberTokenAsync(currentUser, entity.PhoneNumber);
                await _userManager.ChangePhoneNumberAsync(currentUser, entity.PhoneNumber, tokenPhone);

                await _userManager.UpdateAsync(currentUser);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return NotFound();
                }
                return Ok("The restaurant has been successfully updated");
            }
            else
            {
                return Forbid("Only the SuperAdmin or the restaurant themselves can edit this information.");
            }


        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin, Restaurant")]
        //restorani silme
        public async Task<IActionResult> DeleteRestaurant(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (User.IsInRole("SuperAdmin") || currentUser.Id == id)
            {
                Restaurant exactRestaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.Email == currentUser.Email);
                if (exactRestaurant == null)
                {
                    return BadRequest("There is no restaurant to delete");
                }
                _context.Restaurants.Remove(exactRestaurant);
                _context.SaveChanges();
                var userRoles = await _userManager.GetRolesAsync(currentUser);
                foreach (var role in userRoles)
                {
                    await _userManager.RemoveFromRoleAsync(currentUser, role);
                }

        
                var result = await _userManager.DeleteAsync(currentUser);

                if (!result.Succeeded)
                {
                    return BadRequest("Kullanıcı silinemedi.");
                }

                await _context.SaveChangesAsync();
                return Ok("The restaurant has been deleted");
            }
            else
            {
                return Forbid("Only the SuperAdmin or the restaurant themselves can delete this information.");

            }

        }
        [HttpGet("{id}/restaruant-reviews")]
        //restoran yorumlarini getirme
        public async Task<IActionResult> GetRestaurantReviews(int id)
        {
            var restaurant = await _userManager.FindByIdAsync(id.ToString());

            Restaurant? exactRestaurant = await _context.Restaurants.Include(r => r.RestaurantReviews).FirstOrDefaultAsync(r => r.Email == restaurant.Email);
            if (exactRestaurant == null)
            {
                return BadRequest("There is no restaurant with given id");
            }
            else
            {
                return Ok(exactRestaurant.RestaurantReviews.ToList());
            }
        }
        [HttpGet("search-restaurants/{searchQuery}")]
        //restoran arama
        public async Task<IActionResult> SearchRestaurants(string searchQuery)
        {
            return Ok(_context.Restaurants.Where(r => r.Name.Contains(searchQuery)).ToList());
        }
        [HttpGet("get-meals/{restaurantId}")]
        public async Task<IActionResult> GetMealsOfRestaurant(int restaurantId)
        {
            Restaurant? restaurant = await _context.Restaurants.Include(r => r.Meals)
                .FirstOrDefaultAsync(r => r.Id == restaurantId);

            if (restaurant == null)
            {
                return BadRequest("There is no restaurant with the given email.");
            }
            else
            {
                return Ok(restaurant.Meals.ToList());
            }
        }
        [HttpGet("get-meals-inadmin/{restaurantId}")]
        //admin page de restoranin meal lerini getirme
        public async Task<IActionResult> GetMealsOfRestaurantAdmin(int restaurantId)
        {
            var exactRestaurant = await _userManager.FindByIdAsync(restaurantId.ToString());
            if (exactRestaurant == null)
            {
                return BadRequest("Restaurant not found with the given ID.");
            }

            Restaurant? restaurant = await _context.Restaurants.Include(r => r.Meals)
                .FirstOrDefaultAsync(r => r.Email == exactRestaurant.Email);

            if (restaurant == null)
            {
                return BadRequest("There is no restaurant with the given email.");
            }
            else
            {
                return Ok(restaurant.Meals.ToList());
            }
        }
        
    }
}
