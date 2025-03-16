using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using YemekSepeti.DTO;
using YemekSepeti.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace YemekSepeti.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CustomersController : Controller
    {
        private readonly YemekSepetContext _context;
        private UserManager<IdentityUser<int>> _userManager;
        public CustomersController(UserManager<IdentityUser<int>> userManager, YemekSepetContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        [HttpGet]
        //tum musterileri getir
        public async Task<IActionResult> GetAllCustomers()
        {
            List<Customer> customers = await _context.Customers.ToListAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        //spesifik musteriyi getir
        public async Task<IActionResult> GetCustomerById(int? id)
        {
            if (id == null)
            {
                return BadRequest("ID cannot be null");
            }

            IdentityUser<int> customer = await _userManager.FindByIdAsync(id.ToString());
            if (customer == null)
            {
                return BadRequest("Could not find the customer of given ID");
            }

            Customer exactCustomer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == customer.Email);

            if (exactCustomer == null)
            {
                return NotFound("Customer details not found");
            }
            var orderCount = 0;
            decimal totalSpending = 0;

            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "EXEC up_MusterininSiparisSayisiniAl @CustomerId";
                    command.Parameters.Add(new SqlParameter("@CustomerId", exactCustomer.Id));

                    var result = await command.ExecuteScalarAsync();
                    if (result != DBNull.Value)
                    {
                        orderCount = Convert.ToInt32(result);
                    }
                    else
                    {
                        orderCount = 0;  
                    }
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT dbo.MusterininTotalHarcadigiPara(@CustomerId)";
                    command.Parameters.Add(new SqlParameter("@CustomerId", exactCustomer.Id));

                    var result = await command.ExecuteScalarAsync();
                    if (result != DBNull.Value)
                    {
                        totalSpending = Convert.ToDecimal(result);
                    }
                    else
                    {
                        totalSpending = 0m;  
                    }
                }
            }


            var data = new CustomerDTO()
            {
                Id = exactCustomer.Id,
                Name = exactCustomer.Name,
                PhoneNumber = exactCustomer.PhoneNumber,
                Address = exactCustomer.Address,
                OrderCount = orderCount,
                TotalSpending = totalSpending
            };

            return Ok(data);
        }
        [HttpGet("getByEmail/{email}")]
        [Authorize(Roles = "Customer,SuperAdmin")]
        public async Task<IActionResult> GetCustomerByEmail(string? email)
        {
            var customer = await _userManager.FindByEmailAsync(email);
            if (customer == null || email == null)
            {
                return BadRequest("Couldnt find the customer of given id");
            }
            else
            {
                return Ok(customer);
            }
        }
        [Authorize(Roles = "SuperAdmin, Customer")]
        [HttpPut("{id}")]
        //musteri bilgilerini guncelle
        public async Task<IActionResult> EditCustomer(int id, EditCustomerDTO entity)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (User.IsInRole("SuperAdmin") || currentUser.Id == id)
            {
                if (id != id)
                {
                    return BadRequest("The given and entity id don't match.");
                }

                Customer exactCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == currentUser.Email);
                if (exactCustomer == null)
                {
                    return NotFound("There is no customer with the given id!");
                }

                exactCustomer.Name = entity.Name;
                exactCustomer.PhoneNumber = entity.PhoneNumber;
                exactCustomer.Address = entity.Address;

                currentUser.UserName = entity.Name;
                

                var tokenPhone = await _userManager.GenerateChangePhoneNumberTokenAsync(currentUser, entity.PhoneNumber);
                await _userManager.ChangePhoneNumberAsync(currentUser, entity.PhoneNumber, tokenPhone);

                await _userManager.UpdateAsync(currentUser);
                await _context.SaveChangesAsync();
                return Ok("The Customer has been edited successfully");
            }
            else
            {
                return Forbid("Only the SuperAdmin or the customer themselves can edit this information.");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin, Customer")]
        //musteriyi sil
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (User.IsInRole("SuperAdmin") || currentUser.Id == id)
            {

                Customer? exactCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == currentUser.Email);
                if (exactCustomer == null)
                {
                    return NotFound("There is no customer with the given ID to delete.");
                }

                var reviews = _context.RestaurantReviews.Where(r => r.CustomerId == exactCustomer.Id).ToList();
                var orders = _context.Orders.Where(o => o.CustomerId == exactCustomer.Id).ToList();
                    
                if (reviews.Any())
                {
                    _context.RestaurantReviews.RemoveRange(reviews);
                }

                if (orders.Any())
                {
                    _context.Orders.RemoveRange(orders);
                }

                _context.Customers.Remove(exactCustomer);

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


                return Ok("The customer has been deleted successfully.");
            }
            else
            {
                return Forbid("Only the SuperAdmin or the customer themselves can delete this information.");
            }
        }

        [HttpGet("{id}/orders")]
        [Authorize(Roles = "SuperAdmin, Customer")]
        //musterinin siparislerini getir
        public async Task<IActionResult> GetCustomerOrders(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var exactCustomer = _context.Customers.Include(c => c.Orders).FirstOrDefault(c => c.Email == currentUser.Email);
            if (User.IsInRole("SuperAdmin") || currentUser.Id == id)
            {
                if (exactCustomer == null)
                {
                    return NotFound("There is no customer found with given id");
                }
                var orders = await _context.Orders
        .FromSqlRaw("EXEC up_MusteriSiparisleriniAl @CustomerId = {0}", exactCustomer.Id)
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
                return Forbid("Only the SuperAdmin or the customer themselves can get this information.");
            }
        }
        [HttpGet("{id}/reviews")]
        [Authorize(Roles = "SuperAdmin, Customer")]
        //musterinin yorumlarini getir
        public async Task<IActionResult> GetCustomerReviews(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var exactCustomer = _context.Customers.Include(c => c.RestaurantReviews).FirstOrDefault(c => c.Email == currentUser.Email);

            if (User.IsInRole("SuperAdmin") || currentUser.Id == id)
            {
                if (exactCustomer == null)
                {
                    return NotFound("There is no customer found with given id");
                }

                var reviews = await _context.RestaurantReviews
        .FromSqlRaw("EXEC up_MusteriYorumlariniAl @CustomerId = {0}", exactCustomer.Id)
        .ToListAsync();
                var reviews1 = reviews
                .Select(review => new ReviewDTO
                {
                    Id = review.Id,
                    Comment = review.Comment,
                    Rating = review.Rating,
                    ReviewDate = review.ReviewDate,
                    CustomerName = review.Customer != null ? review.Customer.Name : "Anonymous",
                    CustomerId = review.CustomerId
                })
                .ToList();
                return Ok(reviews1);
            }
            else
            {
                return Forbid("Only the SuperAdmin or the customer themselves can get this information.");
            }
        }
        //[HttpPost]
        //public async Task<IActionResult> LoginCustomer(Customer entity)
        //{

        //}
        //Identity add etmemiz lazim
    }
}
