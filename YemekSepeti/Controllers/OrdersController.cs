using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Xml.Linq;
using YemekSepeti.DTO;
using YemekSepeti.Models;

namespace YemekSepeti.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly YemekSepetContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;
        public OrdersController(YemekSepetContext context, UserManager<IdentityUser<int>> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        //tum siparisleri getir
        public async Task<IActionResult> GetAllOrders()
        {
            List<Order> meals = await _context.Orders.ToListAsync();
            return Ok(meals);
        }
        [HttpGet("{id}")]
        //spesifik siparisi getir
        public async Task<IActionResult> GetOrderById(int? id)
        {
            Order? exactOrder = await _context.Orders.FirstOrDefaultAsync(c => c.Id == id);
            if (exactOrder == null || id == null)
            {
                return BadRequest("There is no meal with given id");
            }
            else
            {
                return Ok(exactOrder);
            }
        }
        [HttpPut("{id}/setAsDelivered")]
        [Authorize(Roles = "SuperAdmin, DeliveryPersonnel")]
        //siparis durumunu 'delivered' olarak set etme
        public async Task<IActionResult> SetAsDelivered(int id)
        {
            Order exactOrder = _context.Orders.FirstOrDefault(o => o.Id == id);
            if(exactOrder == null)
            {
                return BadRequest("No order with given id");
            }
            exactOrder.Status = "Delivered";
            _context.SaveChanges();
            return Ok("Order set as delivered!");
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Customer")]
        //siparis verme 
        public async Task<IActionResult> AddOrder([FromBody] OrderDTO entity)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (User.IsInRole("SuperAdmin") || currentUser.Id == entity.CustomerId)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                List<int> idList = new List<int>();
                List<IdentityUser<int>> dPersonnelList = (await _userManager.GetUsersInRoleAsync("DeliveryPersonnel")).ToList();
                foreach (var item in dPersonnelList)
                {
                    idList.Add(item.Id);
                }
                Random rnd = new Random();
                
                List<Meal> newMeals = new List<Meal>();
                idList = idList.OrderBy(x => rnd.Next()).ToList();
                foreach (MealDTO meal in entity.Meals)
                {
                    var restaurant = _context.Restaurants.FirstOrDefault(r => r.Id == meal.RestaurantId);
                    newMeals.Add(new Meal()
                    {
           
                        Name = meal.Name,
                        Price = meal.Price,
                        Description = meal.Description,
                        IsAvailable = meal.IsAvailable,
                        Image = meal.Image,
                        Quantity = meal.Quantity,
                        RestaurantId = meal.RestaurantId,
                        Categories = meal.Categories,
                        Orders = meal.Orders,
                        Restaurant = restaurant
                    });
                }
                var delivery = await _userManager.FindByIdAsync(idList.First().ToString());
                var restaurant1 = await _userManager.FindByIdAsync(entity.RestaurantId.ToString());



                Order newOrder = new Order()
                {
                    OrderDate = entity.OrderDate,
                    TotalAmount = entity.TotalAmount,
                    Status = entity.Status,
                    CustomerId = Convert.ToInt32(entity.CustomerId),
                    RestaurantId = entity.RestaurantId,
                    DeliveryPersonelId = entity.DeliveryPersonelId, 
                    Meals = newMeals,
                    Customer = _context.Customers.FirstOrDefault(c => c.Email == currentUser.Email),
                    Restaurant = _context.Restaurants.FirstOrDefault(r => r.Email == restaurant1.Email),
                    DeliveryPersonel = _context.DeliveryPersonnel.FirstOrDefault(d => d.Email == delivery.Email)
                };

                _context.Orders.Add(newOrder);
                await _context.SaveChangesAsync(); 

   
                var payment = new Payment()
                {
                    PaymentMethod = entity.PaymentMethod,
                    PaymentDate = DateTime.UtcNow,
                    Amount = entity.TotalAmount,
                    IsSuccessful = true,
                    OrderId = newOrder.Id,
                    Order = newOrder
                };

                newOrder.Payments = new List<Payment> { payment };
                await _context.SaveChangesAsync(); 

                return Ok("The new order has been added");
            }
            else
            {
                return Forbid("Only the SuperAdmin or the customer themselves can add this information.");
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin, Customer,Restaurant")]
        //order iptal etme
        public async Task<IActionResult> CancelOrder(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);


            Order? exactOrder = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (exactOrder == null)
            {
                return BadRequest("There is no order with given id!");
            }
            if (exactOrder.Status == "Delivered")
            {
                return BadRequest("You can't cancel the delivered order");
            }
            else
            {
                if (User.IsInRole("SuperAdmin") || (currentUser.Id == exactOrder.CustomerId || currentUser.Id == exactOrder.RestaurantId))
                {
                    _context.Orders.Remove(exactOrder);
                    await _context.SaveChangesAsync();
                    return Ok("The order has been successfully cancelled");
                }
                else
                {
                    return Forbid("Only the SuperAdmin or the customer or the restaurant themselves can delete this information.");
                }

            }
        }
        [HttpGet("{restaurantId}/ordersByRestaurant")]
        [Authorize(Roles = "SuperAdmin,Restaurant")]
        //restoranlarin siparislerini alma
        public async Task<IActionResult> GetOrdersByRestaurant(int restaurantId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (User.IsInRole("SuperAdmin") || currentUser.Id == restaurantId)
            {
                var restaurant = await _context.Restaurants
                    .Include(r => r.Orders)
                    .FirstOrDefaultAsync(r => r.Email == currentUser.Email);

                if (restaurant == null)
                {
                    return BadRequest("There is no restaurant with the given id!");
                }
                var orders = await _context.Orders
            .FromSqlRaw("EXEC up_RestoranSiparisleriniAl @RestaurantId = {0}", restaurantId)
            .ToListAsync();
              
                var orders1 = orders.Select(o => new OrderDTO
                {
                    TotalAmount = o.TotalAmount,
                    Status = o.Status, 
                    OrderDate = o.OrderDate,
                    CustomerId = o.CustomerId,
                    RestaurantId = o.RestaurantId
                }).ToList();

                return Ok(orders1);
            }
            else
            {
                return Forbid("Only the SuperAdmin or the restaurant themselves can delete this information.");
            }
        }


    }
}
