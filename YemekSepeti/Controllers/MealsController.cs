using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YemekSepeti.Models;
using YemekSepeti.DTO;
using Microsoft.Data.SqlClient;

namespace YemekSepeti.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MealsController : Controller
    {
        private readonly YemekSepetContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly string _connectionString;
        public MealsController(YemekSepetContext context, UserManager<IdentityUser<int>> userManager,IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _connectionString = configuration.GetConnectionString("YemekSepeti");
        }
        [HttpGet]
        //tum yiyecekleri alma
        public async Task<IActionResult> GetAllMeals()
        {
            //List<Meal> meals = await _context.Meals.ToListAsync();
            //return Ok(meals);

            List<Meal> meals = new List<Meal>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT Id, Name, Price, Description, IsAvailable, Image, Quantity, RestaurantId FROM Meals";

                SqlCommand command = new SqlCommand(query, connection);
                await connection.OpenAsync();

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while(await reader.ReadAsync())
                    {
                        Meal meal = new Meal() { 
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Price = reader.GetDecimal(2),
                            Description = reader.GetString(3),
                            IsAvailable = reader.GetBoolean(4),
                            Image = reader.GetString(5),
                            Quantity = reader.GetInt32(6),
                            RestaurantId = reader.GetInt32(7)
                        };
                        meals.Add(meal);

                    }
                }
            }
            return Ok(meals);
        }
        [HttpGet("{id}")]
        //spesifik yiyecegi alma
        public async Task<IActionResult> GetMealById(int? id)
        {
            //Meal exactMeal = await _context.Meals.FirstOrDefaultAsync(c => c.Id == id);
            //if (exactMeal == null || id == null)
            //{
            //    return BadRequest("There is no meal with given id");
            //}
            //else
            //{
            //    return Ok(exactMeal);
            //}

            Meal meal = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @" SELECT m.Id AS Id, m.Name as Name, m.Price as Price, m.Description as Description, m.IsAvailable as IsAvailable, m.Image as Image, m.Quantity AS Quantity, m.RestaurantId as RestaurantId FROM MEALS m WHERE m.Id = @MealId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MealId", id);

                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if(await reader.ReadAsync())
                    {
                        meal = new Meal { 
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable")),
                            Image = reader.GetString(reader.GetOrdinal("Image")),
                            Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                            RestaurantId = reader.GetInt32(reader.GetOrdinal("RestaurantId")),
                        };
                    }
                }
            }
            if(meal == null)
            {
                return NotFound();
            }
            return Ok(meal);
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Restaurant")]
        //yemek ekleme
        public async Task<IActionResult> AddMeal(MealDTO entity)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var restaurantItself = await _context.Restaurants.FirstOrDefaultAsync(r => r.Email == currentUser.Email);

            if (User.IsInRole("SuperAdmin") || currentUser.Id == entity.RestaurantId)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Meal newEntity = new Meal() { 
                    Name = entity.Name,
                    Price = entity.Price,
                    Description = entity.Description,
                    IsAvailable = entity.IsAvailable,
                    Image = entity.Image,
                    Quantity = entity.Quantity,
                    RestaurantId = entity.RestaurantId,
                    Restaurant = restaurantItself,
                    Categories = entity.Categories,
                    Orders = entity.Orders
                };

                _context.Meals.Add(newEntity);
                await _context.SaveChangesAsync();
                return Ok("The new meal has been added");
            }
            else
            {
                return Forbid("Only the SuperAdmin or the restaurant themselves can add this information.");
            }

        }
        [HttpPost("{id}")]
        //yemek icerigini guncelleme
        public async Task<IActionResult> UpdateMeal(int? id, Meal entity)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (User.IsInRole("SuperAdmin") || currentUser.Id == entity.RestaurantId)
            {
                if (id != entity.Id)
                {
                    return BadRequest("Ids are not matching");
                }
                Meal exactMeal = await _context.Meals.FirstOrDefaultAsync(m => m.Id == id);
                if (exactMeal == null)
                {
                    return BadRequest("There is no meal with given id");
                }
                exactMeal.Name = entity.Name;
                exactMeal.Price = entity.Price;
                exactMeal.Orders = entity.Orders;
                exactMeal.Restaurant = entity.Restaurant;
                exactMeal.RestaurantId = entity.RestaurantId;
                exactMeal.Categories = entity.Categories;
                exactMeal.Description = entity.Description;
                exactMeal.IsAvailable = entity.IsAvailable;
                exactMeal.Image = entity.Image;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return NotFound();
                }
                return Ok("The meal has been successfully updated");
            }
            else
            {
                return Forbid("Only the SuperAdmin or the restaurant themselves can edit this information.");
            }

        }
        [HttpDelete("{id}")]
        //yemek silme
        public async Task<IActionResult> DeleteMeal(int? id)
            {
            var currentUser = await _userManager.GetUserAsync(User);


            Meal exactMeal = await _context.Meals.FirstOrDefaultAsync(c => c.Id == id);
            var currentRestaurant = _context.Restaurants.FirstOrDefault(r => r.Id == exactMeal.RestaurantId);
            if (exactMeal == null || id == null)
            {
                return BadRequest("We couldt find any product with that id");
            }
            if (User.IsInRole("SuperAdmin") || currentUser.Email == currentRestaurant.Email)
            {
                _context.Meals.Remove(exactMeal);
                await _context.SaveChangesAsync();
                return Ok("The meal has been deleted successfully");
            }
            else
            {
                return Forbid("Only the SuperAdmin or the restaurant themselves can delete this information.");
            }

        }

        [HttpGet("/searchMeal/{query}")]
        public async Task<IActionResult> SearchMeal(string query)
        {
            List<Meal> meals = _context.Meals.Where(m => m.Name.Contains(query)).ToList();
            if(meals.Count() == 0)
            {
                return Ok($"There is no meal called '{query}'");
            }
            return Ok(meals);
        } 
    }
}
