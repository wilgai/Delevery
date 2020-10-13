using Del.Common.Requests;
using Delevery.Web.Data;
using Delevery.Web.Data.Entities;
using Delevery.Web.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Delevery.Web.Controllers.API
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class QualificationsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public QualificationsController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        [HttpPost]
        public async Task<IActionResult> PostQualification([FromBody] QualificationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            Restaurant restaurant = await _context.Restaurants
                .Include(p => p.Qualifications)
                .FirstOrDefaultAsync(p => p.Id == request.RestaurantId);
            if (restaurant == null)
            {
                return NotFound("Error002");
            }

            if (restaurant.Qualifications == null)
            {
                restaurant.Qualifications = new List<Qualification>();
            }

            restaurant.Qualifications.Add(new Qualification
            {
                Date = DateTime.UtcNow,
                Restaurant = restaurant,
                Remarks = request.Remarks,
                Score = request.Score,
                User = user
            });

            _context.Restaurants.Update(restaurant);
            await _context.SaveChangesAsync();
            return Ok(restaurant);
        }
    }

}
