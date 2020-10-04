using Delevery.Web.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delevery.Web.Helpers
{
    public class CategoryCombosHelper : ICategoryCombosHelper
    {
        private readonly DataContext _context;

        public CategoryCombosHelper(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetComboCategories()
        {
            List<SelectListItem> list = _context.Categories.Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = $"{t.Id}"
            })
                .OrderBy(t => t.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Selecciona una categoría...]",
                Value = "0"
            });

            return list;
        }

    }
}
