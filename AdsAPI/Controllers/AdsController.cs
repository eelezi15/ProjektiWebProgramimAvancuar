using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdsAPI.Models;

namespace AdsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdsController : ControllerBase
    {
        private readonly AdsAPIDataContext _context;

        public AdsController(AdsAPIDataContext context)
        {
            _context = context;
        }

        // GET: api/Ads
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ad>>> GetAd()
        {
          if (_context.Ad == null)
          {
              return NotFound();
          }
            return await _context.Ad.ToListAsync();
        }

        // GET: api/Ads/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ad>> GetAd(Guid id)
        {
          if (_context.Ad == null)
          {
              return NotFound();
          }
            var ad = await _context.Ad.FindAsync(id);

            if (ad == null)
            {
                return NotFound();
            }

            return ad;
        }

        // PUT: api/Ads/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAd(Guid id, Ad ad)
        {
            if (id != ad.AdId)
            {
                return BadRequest();
            }

            _context.Entry(ad).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdExists(id))
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

        // POST: api/Ads
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ad>> PostAd(Ad ad)
        {
          if (_context.Ad == null)
          {
              return Problem("Entity set 'AdsAPIDataContext.Ad'  is null.");
          }
            _context.Ad.Add(ad);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAd", new { id = ad.AdId }, ad);
        }

        // DELETE: api/Ads/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAd(Guid id)
        {
            if (_context.Ad == null)
            {
                return NotFound();
            }
            var ad = await _context.Ad.FindAsync(id);
            if (ad == null)
            {
                return NotFound();
            }

            _context.Ad.Remove(ad);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdExists(Guid id)
        {
            return (_context.Ad?.Any(e => e.AdId == id)).GetValueOrDefault();
        }
    }
}
