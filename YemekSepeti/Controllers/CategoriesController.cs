using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YemekSepeti.Models;

namespace YemekSepeti.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CategoriesController : Controller
    {
        private readonly YemekSepetContext _context;
        public CategoriesController(YemekSepetContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            List<Category> categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int? id)
        {

            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null || id == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(category);
            }
        }
        [HttpGet("{id}/meals")]
        public async Task<IActionResult> GetMealsByCategory(int? id)
        {

            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null || id == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(category.Meals.ToList());
            }
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AddCategory(Category entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Category category = new Category()
            {
                Name = entity.Name,
                Meals = entity.Meals,
                Restaurants = entity.Restaurants
            };
            _context.Categories.Add(category);
            _context.SaveChanges();
            return Ok("Category has been added");
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> UpdateCategory(int id, Category entity)
        {
            if (id != entity.Id)
            {
                return BadRequest("Ids are not matching!");
            }
            Category exactCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (exactCategory == null)
            {
                return BadRequest($"There is no category with {id} id");
            }
            exactCategory.Name = entity.Name;
            exactCategory.Restaurants = entity.Restaurants;
            exactCategory.Meals = entity.Meals;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return NotFound();
            }
            return Ok("The category has been successfully updated");

        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteCategory(int? id)
        {
            Category exactCategory = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (exactCategory == null || id == null)
            {
                return BadRequest("There is no category to delete with given id");
            }
            else
            {
                _context.Categories.Remove(exactCategory);
                await _context.SaveChangesAsync();
                return Ok("The Category has been deleted");
            }
        }
    }
}
