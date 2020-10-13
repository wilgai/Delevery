using Del.Common.Entities;
using Delevery.Web.Data;
using Delevery.Web.Data.Entities;
using Delevery.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delevery.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
   
        public Restaurant ToRestaurant(RestaurantViewModel model, Guid imageId, bool isNew)
        {
            return new Restaurant
            {
                Id = isNew ? 0 : model.Id,
                ImageId = imageId,
                Name = model.Name,
                Address=model.Address
            };
        }

        public RestaurantViewModel ToRestaurantViewModel(Restaurant restaurant)
        {
            return new RestaurantViewModel
            {
                Id = restaurant.Id,
                ImageId = restaurant.ImageId,
                Name = restaurant.Name,
                Address=restaurant.Address
            };
        }

        
    }

}

