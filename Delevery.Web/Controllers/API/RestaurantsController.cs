using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Delevery.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Delevery.Web.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]

    public class RestaurantsController : Controller
    {
            private readonly DataContext _context;

            public RestaurantsController(DataContext context)
            {
                _context = context;
            }

            [HttpGet]
            public IActionResult GetRestaurants()
            {
                return Ok(_context.Restaurants
                    .Include(p => p.Qualifications)
                    .OrderBy(p => p.Name)
                    );
            }
        

    }
}
