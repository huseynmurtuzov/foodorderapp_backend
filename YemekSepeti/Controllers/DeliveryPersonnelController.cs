using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using YemekSepeti.DTO;
using YemekSepeti.Models;

namespace YemekSepeti.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryPersonnelController : Controller
    {
        private readonly YemekSepetContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly string _connectionString;
        public DeliveryPersonnelController(UserManager<IdentityUser<int>> userManager, YemekSepetContext context, IConfiguration configuration)
        {
            _userManager = userManager;
            _context = context;
            _connectionString = configuration.GetConnectionString("YemekSepeti");
        }

        [HttpGet]
        //tum kuryelerin detaylarini getir
        public async Task<IActionResult> GetAllDeliveryPersonnel()
        {
            return Ok(_context.DeliveryPersonnel.ToList());
        }
        [HttpGet("{id}")]
        //spesifik kurye detaylarini getir
        public async Task<IActionResult> GetDeliveryPersonnelById(int id)
        {
            IdentityUser<int> delivery = await _userManager.FindByIdAsync(id.ToString());
            DeliveryPersonnel? exactDeliveryPersonnel = await _context.DeliveryPersonnel
                .Include(d => d.Orders)
                .FirstOrDefaultAsync(d => d.Email == delivery.Email);

            if (exactDeliveryPersonnel == null)
            {
                return BadRequest("There is no delivery personnel with given id");
            }

            DeliveryPersonnelDTO data = new DeliveryPersonnelDTO()
            {
                Id = exactDeliveryPersonnel.Id,
                Name = exactDeliveryPersonnel.Name,
                PhoneNumber = exactDeliveryPersonnel.PhoneNumber,
                VeichleType = exactDeliveryPersonnel.VeichleType,
                Orders = exactDeliveryPersonnel.Orders
                    .Select(o => new DeliveryOrderDTO
                    {
                        Id = o.Id,
                        OrderDate = o.OrderDate,
                        Status = o.Status,
                        TotalAmount = o.TotalAmount,
                        CustomerId = o.CustomerId,
                        RestaurantId = o.RestaurantId,
                    })
                    .ToList()
            };

            return Ok(data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin, DeliveryPersonnel")]
        //kurye bilgilerini guncelle
        public async Task<IActionResult> UpdateDeliveryPersonnel([FromRoute] int id, [FromBody] EditDeliveryPersonnelDTO entity)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (User.IsInRole("SuperAdmin") || currentUser.Id == id)
            {
                if (currentUser.Id != id)
                {
                    return BadRequest("Given and entity ids doesnt match");
                }
                DeliveryPersonnel? exactDeliveryPersonnel = await _context.DeliveryPersonnel.FirstOrDefaultAsync(d => d.Email == currentUser.Email);
                if (exactDeliveryPersonnel == null)
                {
                    return BadRequest("There is no delivery personnel with given id");
                }
                exactDeliveryPersonnel.Name = entity.Name;
                exactDeliveryPersonnel.PhoneNumber = entity.PhoneNumber;
                exactDeliveryPersonnel.VeichleType = entity.VeichleType;


                currentUser.UserName = entity.Name;


                var tokenPhone = await _userManager.GenerateChangePhoneNumberTokenAsync(currentUser, entity.PhoneNumber);
                await _userManager.ChangePhoneNumberAsync(currentUser, entity.PhoneNumber, tokenPhone);

                await _userManager.UpdateAsync(currentUser);

                await _context.SaveChangesAsync();
                return Ok("The delivery personnel has been edited successfully!");
            }
            else
            {
                return Forbid("Only the SuperAdmin or the Delivery Personnel themselves can edit this information.");
            }

        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin, DeliveryPersonnel")]
        //kuryeyi sil
        public async Task<IActionResult> DeleteDeliveryPersonnel(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (User.IsInRole("SuperAdmin") || currentUser.Id == id)
            {
                DeliveryPersonnel? exactDeliveryPersonnel = await _context.DeliveryPersonnel.FirstOrDefaultAsync(d => d.Email == currentUser.Email);
                if (exactDeliveryPersonnel == null)
                {
                    return BadRequest("There is no delivery personnel with given id");
                }
                _context.DeliveryPersonnel.Remove(exactDeliveryPersonnel);
                await _context.SaveChangesAsync();
                var userRoles = await _userManager.GetRolesAsync(currentUser);
                foreach (var role in userRoles)
                {
                    await _userManager.RemoveFromRoleAsync(currentUser, role);
                }

                // Kullanıcıyı silme işlemi
                var result = await _userManager.DeleteAsync(currentUser);

                if (!result.Succeeded)
                {
                    return BadRequest("Kullanıcı silinemedi.");
                }

                await _context.SaveChangesAsync();

                return Ok("The delivery personnel has been deleted successfully");
            }
            else
            {
                return Forbid("Only the SuperAdmin or the Delivery Personnel themselves can delete this information.");
            }

        }


        //[HttpGet("performance/{deliveryPersonnelId}")]
        //public async Task<IActionResult> GetDeliveryPersonnelPerformanceById(int deliveryPersonnelId)
        //{
        //    var currentUser = await _userManager.FindByIdAsync(deliveryPersonnelId.ToString());
        //    var exactDeliveryPersonnel = _context.DeliveryPersonnel.FirstOrDefault(d => d.Email == currentUser.Email);
        //    var performances = await _context.DeliveryPersonnel
        //        .FromSqlRaw("SELECT * FROM vw_DeliveryPersonnelPerformance WHERE DeliveryPersonnelId = {0}", exactDeliveryPersonnel.Id)
        //        .Select(dp => new DeliveryPersonnelPerformance
        //        {
        //            DeliveryPersonnelId = EF.Property<int>(dp, "DeliveryPersonnelId"),
        //            DeliveryPersonnelName = EF.Property<string>(dp, "DeliveryPersonnelName"),
        //            TotalDeliveredOrders = EF.Property<int>(dp, "TotalDeliveredOrders")
        //        })
        //        .ToListAsync();

        //    if (!performances.Any())
        //    {
        //        return NotFound("No performance data found for the specified delivery personnel.");
        //    }

        //    return Ok(performances.FirstOrDefault());


        //}

        [HttpGet("performance/{deliveryPersonnelId}")]
        //kuryenin performansini view olarak gonder
        public async Task<object> GetDeliveryPersonnelPerformanceById(int deliveryPersonnelId)
        {
            var currentUser = await _userManager.FindByIdAsync(deliveryPersonnelId.ToString());
            var exactUser = _context.DeliveryPersonnel.FirstOrDefault(d => d.Email == currentUser.Email);
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
            SELECT 
                DeliveryPersonnelId, 
                DeliveryPersonnelName, 
                TotalDeliveredOrders
            FROM vw_DeliveryPersonnelPerformance
            WHERE DeliveryPersonnelId = @DeliveryPersonnelId";

                using (var command = new SqlCommand(query, connection))
                {
                    // Parametre ekle
                    command.Parameters.Add(new SqlParameter("@DeliveryPersonnelId", exactUser.Id));

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var data = new DeliveryPersonnelPerformance
                            {
                                DeliveryPersonnelId = Convert.ToInt32(reader["DeliveryPersonnelId"]),
                                DeliveryPersonnelName = reader["DeliveryPersonnelName"].ToString(),
                                TotalDeliveredOrders = Convert.ToInt32(reader["TotalDeliveredOrders"])
                            };
                            return data;
                        }
                    }
                }
            }

        return null;
        }
    }
}