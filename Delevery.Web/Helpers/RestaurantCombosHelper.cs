using Delevery.Web.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delevery.Web.Helpers
{
    public class RestaurantCombosHelper : IRestaurantCombosHelper
    {

        private readonly DataContext _context;
        public RestaurantCombosHelper(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetComboRestaurants()
        {
            List<SelectListItem> list = _context.Restaurants.Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = $"{t.Id}"
            })
                .OrderBy(t => t.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Selecciona un restaurante...]",
                Value = "0"
            });

            return list;
        }
    }
}
