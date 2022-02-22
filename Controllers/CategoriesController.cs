using ASPNETLIVE.Data;
using ASPNETLIVE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPNETLIVE.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly APIContext _context;

        public CategoriesController(APIContext context)
        {
            _context = context;
        }

        // GET: api/Category
        //[HttpGet]
        //public async Task<IActionResult> GetCategory()
        //{
        //    //var category = await _context.Category.AsNoTracking().ToListAsync(); // อยากให้ทำงานเร็วขึ้น => ไม่ได้เอาข้อมูลไปทำอะไร .AsNoTracking()
        //    //var category = await _context.Category.OrderByDescending(c => c.CategoryId).AsNoTracking().ToListAsync(); // OrderByDescending
        //    var category = await _context.Category.FromSqlRaw("select * from Category order by CategoryId").ToListAsync(); // SQL

        //    var total = await _context.Category.CountAsync();

        //    return Ok(new { totalRow = total, data = category });
        //}

        //Pagination
        //api/category
        //api/category?page=1&pageSize=3
        [HttpGet]
        public async Task<IActionResult> GetCategory(int page = 1, int pageSize = 3)
        {
            //var category = await _context.Category.AsNoTracking().ToListAsync(); // อยากให้ทำงานเร็วขึ้น => ไม่ได้เอาข้อมูลไปทำอะไร .AsNoTracking()
            var category = await _context.Category
                .OrderByDescending(c => c.CategoryId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync(); // OrderByDescending || Skip,Take => การแบ่งหน้า
            //var category = await _context.Category.FromSqlRaw("select * from Category order by CategoryId").ToListAsync(); // SQL

            var total = await _context.Category.CountAsync();

            return Ok(new { totalRow = total, data = category });
        }

        // GET: api/Category/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _context.Category.FindAsync(id);

            if (category == null)
            {
                return NotFound(new { message = "ไม่พบข้อมูลนี้ในระบบ" });
            }

            return Ok(category);
        }

        // PUT: api/Category/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest(new { message = "Id ที่แก้ไขไม่ตรงกัน" });
            }

            _context.Entry(category).State = EntityState.Modified; // เปิดให้สามารถแก้ไขได้

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Category
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            await _context.Category.AddAsync(category);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetCategory", new { id = category.CategoryId }, category);
            return Created("", new { message = "เพิ่มข้อมูลสำเร็จ" });
        }

        // DELETE: api/Category/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound(new { message = "ไม่พบข้อมูลนี้ในระบบ" });
            }

            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.CategoryId == id);
        }
    }
}
