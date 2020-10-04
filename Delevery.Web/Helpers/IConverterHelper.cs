using Del.Common.Entities;
using Delevery.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delevery.Web.Helpers
{
    public interface IConverterHelper
    {
        Restaurant ToRestaurant(RestaurantViewModel model, Guid imageId, bool isNew);

        RestaurantViewModel ToRestaurantViewModel(Restaurant restaurant);
    }
}
