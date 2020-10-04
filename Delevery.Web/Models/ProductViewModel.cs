using Del.Common.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Delevery.Web.Models
{

    public class ProductViewModel : Product
    {
        [Display(Name = "Categoria")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una categoría.")]
        [Required]
        public int CategoryId { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }

        [Display(Name = "Restaurante")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un restaurante.")]
        [Required]
        public int RestaurantId { get; set; }
        public IEnumerable<SelectListItem> Restaurants { get; set; }

        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }

}
