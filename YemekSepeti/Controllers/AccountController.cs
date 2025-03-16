using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YemekSepeti.DTO;
using YemekSepeti.Models;

namespace YemekSepeti.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly YemekSepetContext _context;
        private readonly IConfiguration _configuration;
        private UserManager<IdentityUser<int>> _userManager;
        //private RoleManager<IdentityRole<int>> _roleManager;
        private SignInManager<IdentityUser<int>> _signInManager;

        public AccountController(YemekSepetContext context, UserManager<IdentityUser<int>> userManager, SignInManager<IdentityUser<int>> signInManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO data)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(data.Email);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    var result = await _signInManager.PasswordSignInAsync(user, data.Password, true, true);
                    if (result.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(user);
                        await _userManager.SetLockoutEndDateAsync(user, null);

                        return Ok(
                            new { token = GenerateJWT(user) }
                            );
                    }
                    else if (result.IsLockedOut)
                    {
                        var lockoutDate = await _userManager.GetLockoutEndDateAsync(user);
                        var timeLeft = lockoutDate.Value - DateTime.UtcNow;
                        ModelState.AddModelError("", $"Your account has been locked. Try again after {timeLeft.TotalMinutes} minutes");
                        return BadRequest(ModelState);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Wrong Password!");
                        return BadRequest(ModelState);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User not found");
                    return BadRequest(ModelState);
                }
            }
            else
            {
                ModelState.AddModelError("", "Wrong email or password");
                return BadRequest(ModelState);
            }
        }

        private object GenerateJWT(IdentityUser<int> user)
        {
            var role = _userManager.GetRolesAsync(user).Result;
            var key = _configuration.GetSection("AppSettings:Secret").Value;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims:
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName ?? ""),
                        new Claim(ClaimTypes.Role, _userManager.GetRolesAsync(user).Result.FirstOrDefault() ?? ""),
                        new Claim(ClaimTypes.Email, user.Email),
                    },
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsCustomer(CustomerRegisterDTO data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IdentityUser<int> user = new IdentityUser<int>()
            {
                UserName = data.Name,
                Email = data.Email,
                PhoneNumber = data.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            Customer entity = new Customer()
            {
                Email = data.Email,
                Password = data.Password,
                Name = data.Name,
                PhoneNumber = data.PhoneNumber,
                Address = data.Address,
                Orders = new List<Order>(),
                RestaurantReviews = new List<RestaurantReview>(),
                Restaurants = new List<Restaurant>()
            };

            var result = await _userManager.CreateAsync(user, data.Password);
            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "Customer");
                if (!roleResult.Succeeded)
                {
                    return BadRequest(roleResult.Errors);
                }

                _context.Customers.Add(entity);
                await _context.SaveChangesAsync();

                return StatusCode(201);
            }
            else
            {
                return BadRequest(result.Errors);
            }

        }
        [HttpPost("Register/Restaurant")]
        public async Task<IActionResult> RegisterAsRestaurant(RestaurantRegisterDTO data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IdentityUser<int> user = new IdentityUser<int>()
            {
                UserName = data.Name,
                Email = data.Email,
                PhoneNumber = data.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString()

            };
            var result = await _userManager.CreateAsync(user, data.Password);
            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "Restaurant");
                if (!roleResult.Succeeded)
                {
                    return BadRequest(roleResult.Errors);
                }

                _context.Restaurants.Add(new Restaurant()
                {
                    Name = data.Name,
                    Email = data.Email,
                    PhoneNumber = data.PhoneNumber,
                    Password = data.Password,
                    Address = data.Address,
                    WorkingHours = data.WorkingHours,
                    Rating = data.Rating,
                    Image = data.Image,
                    Meals = new List<Meal>(),
                    Categories = new List<Category>(),
                    Customers = new List<Customer>(),
                    Orders = new List<Order>(),
                    RestaurantReviews = new List<RestaurantReview>()

                });
                ;
                await _context.SaveChangesAsync();

                return StatusCode(201);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
        [HttpPost("Register/DeliveryPersonnel")]
        public async Task<IActionResult> RegisterAsDeliveryPersonnel(DeliveryPersonnelRegisterDTO data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IdentityUser<int> user = new IdentityUser<int>()
            {
                UserName = data.Name,
                Email = data.Email,
                PhoneNumber = data.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, data.Password);
            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "DeliveryPersonnel");
                if (!roleResult.Succeeded)
                {
                    return BadRequest(roleResult.Errors);
                }

                _context.DeliveryPersonnel.Add(new DeliveryPersonnel()
                {
                    Name = data.Name,
                    Email = data.Email,
                    Password = data.Password,
                    PhoneNumber = data.PhoneNumber,
                    VeichleType = data.VeichleType,
                    Orders = new List<Order>(),

                });
                ;
                await _context.SaveChangesAsync();

                return StatusCode(201);
            }
            else
            {
                return BadRequest(result.Errors);
            }

        }
        //[HttpGet("GetCurrentUser")]
        //public async Task<AppUser> GetCurrentUser(LoginDTO data)
        //{
        //    AppUser currentUser = await _userManager.FindByEmailAsync(data.Email);
        //    return currentUser;
        //}
    }
}
