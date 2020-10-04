using Del.Common.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Delevery.Web.Models
{
    public class RestaurantViewModel:Restaurant
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }



    }
}
