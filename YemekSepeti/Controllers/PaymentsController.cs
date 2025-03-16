using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YemekSepeti.Models;

namespace YemekSepeti.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : Controller
    {
        private readonly YemekSepetContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;
        public PaymentsController(YemekSepetContext context, UserManager<IdentityUser<int>> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        [HttpGet]
        [Authorize("SuperAdmin")]
        public async Task<IActionResult> GetAllPayments()
        {
            List<Payment> payments = await _context.Payments.ToListAsync();
            return Ok(payments);
        }
        [HttpGet("{id}")]
        [Authorize("SuperAdmin")]
        public async Task<IActionResult> GetPaymentById(int? id)
        {
            Payment? exactPayment = await _context.Payments.FirstOrDefaultAsync(c => c.Id == id);
            if (exactPayment == null || id == null)
            {
                return BadRequest("There is no payment with given id");
            }
            else
            {
                return Ok(exactPayment);
            }
        }
        [HttpPost]
        [Authorize("Customer")]
        public async Task<IActionResult> ProccesPayment(Payment entity)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id == entity.Order.CustomerId)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _context.Payments.Add(entity);
                await _context.SaveChangesAsync();
                return Ok("The new payment has been added");
            }
            else
            {
                return Forbid("Only the Customer can procces this payment.");
            }
        }

        [HttpPost("{id}")]
        [Authorize("Customer")]
        public async Task<IActionResult> VerifyPayment(int? id)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            Payment? exactPayment = await _context.Payments.FirstOrDefaultAsync(m => m.Id == id);
            if (exactPayment == null)
            {
                return BadRequest("There is no payment with given id");
            }
            if (currentUser.Id == exactPayment.Order.CustomerId)
            {
                exactPayment.IsSuccessful = true;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return NotFound();
                }
                return Ok("The payment has been verified updated");
            }
            else
            {
                return Forbid("Only the Customer can verify this payment.");
            }

        }
    }
}
