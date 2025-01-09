using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjektiWebProgramimAvancuar.Data;
using ProjektiWebProgramimAvancuar.Models;

namespace ProjektiWebProgramimAvancuar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ProjektiWebProgramimAvancuarContext _context;

        public PostsController(ProjektiWebProgramimAvancuarContext context)
        {
            _context = context;
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPost()
        {
          if (_context.Post == null)
          {
              return NotFound();
          }
            return await _context.Post.Include(p => p.Comments).Include(p => p.User).ToListAsync();
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(Guid id)
        {
          if (_context.Post == null)
          {
              return NotFound();
          }
            var post = await _context.Post.Include(p => p.Comments).Include(p=>p.User).FirstOrDefaultAsync(p => p.PostId == id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        // PUT: api/Posts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(Guid id, Post post)
        {
            if (id != post.PostId)
            {
                return BadRequest();
            }

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
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

        // POST: api/Posts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(Post post)
        {
          if (_context.Post == null)
          {
              return Problem("Entity set 'ProjektiWebProgramimAvancuarContext.Post'  is null.");
          }
            _context.Post.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPost", new { id = post.PostId }, post);
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            if (_context.Post == null)
            {
                return NotFound();
            }
            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Post.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("PostsByUser/{id}")]

        public async Task<ActionResult<IEnumerable<Post>>> PostsByUser(Guid id)
        {
            if(_context.Post == null)
            {
                return Problem("Nuk ekziston");
            }

            var postbyuser= await  _context.Post.Include(p => p.User).Include(p => p.Comments).Where(p => p.UserId == id).ToListAsync();

            if(postbyuser == null)
            {
                return Problem("Not found");
            }
            return Ok(postbyuser);

        }

        [HttpPost("IncreaseViewCount/{id}")]

        public async Task<ActionResult<Post>> IncreaseViewCount(Guid id)
        {
            if(_context.Post == null)
            {
                return Problem("Nuk ekziston tabela Post");
            }

            var post = await _context.Post.FindAsync(id);

            if(post == null)
            {
                return NotFound("Nuk ekziston ky post");
            }

            post.ViewCount++;
            await _context.SaveChangesAsync();

            return Ok(post);
        }

        [HttpGet("TopViewedPostsLastThreeMonths")]

        //Declares an asynchronous function that returns a list of MonthlyPosts (class).
        //It will contain data about the top 3 most-viewed posts for the last 3 months.
        public async Task<ActionResult<IEnumerable<MonthlyPosts>>> GetTopViewedPostsLastThreeMonths()
        {
            //Checks if the Post table in the database is null (doesn't exist). If so it returns a message
            if (_context.Post == null)
            {
                return Problem("Nuk ekziston tabela");
            }

            //Stores the current date and time in UTC format.
            var currentDate = DateTime.UtcNow;
            //Creates an empty list of MonthlyPosts that will hold the top 3 viewed posts for each of the last 3 months.
            var results = new List<MonthlyPosts>();

            //A loop that runs 3 times, once for each of the last 3 months.
            for (int i = 0; i < 3; i++)
            {
                //Calculates the first day of the current month, then subtracts i months to get the first day of the respective month in the loop.
                var startOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(-i);
                //Calculates the last day of the same month by adding one month to startOfMonth and subtracting one day.
                var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                // Get the top 3 viewed posts for the current month
                //Queries the Post table to find posts created between startOfMonth and endOfMonth
                var topPosts = await _context.Post

                    .Where(p => p.CreatedAt >= startOfMonth && p.CreatedAt <= endOfMonth)// Filters posts within the month range.
                    .OrderByDescending(p => p.ViewCount)//Orders the posts by ViewCount in descending order (highest views first).
                    .Take(3)// Selects only the top 3 posts from the ordered list.
                    .Select(p => new PostSummary//Creates a PostSummary object (only PostId and Title are selected for each post).  
                    {
                        //We assign the post values to the PostSummary properties
                        PostId = p.PostId,
                        Title = p.Title,
                        ViewCount = p.ViewCount

                    })
            .ToListAsync();

                //We add a new monthly posts object to the results list
                results.Add(new MonthlyPosts
                {
                    Month = startOfMonth.ToString("MMMM yyyy"),
                    Posts = topPosts
                });
            }

            return Ok(results);
        }

        [HttpGet("GetPostsBySearchText")]

        public async Task<ActionResult<IEnumerable<Post>>> GetPostsBySearchText([FromQuery] string searchText)
        {
            if (_context.Post == null)
            {
                return Problem("Nuk ekziston tabela Post");
            }

            var posts = await _context.Post.Include(p => p.User).Include(p => p.Comments).Where(p => p.Title.Contains(searchText) || p.Content.Contains(searchText)).ToListAsync();

            if (posts == null)
            {
                return NotFound("Nuk u gjet asgje");
            }

            return Ok(posts);
        }
        private bool PostExists(Guid id)
        {
            return (_context.Post?.Any(e => e.PostId == id)).GetValueOrDefault();
        }
    }
}
