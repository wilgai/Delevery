using Del.Common.Entities;
using Delevery.Web.Data.Entities;
using Delevery.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delevery.Web.Helpers
{
    public interface ICategoryConverterHelper
    {
        Category ToCategory(CategoryViewModel model, Guid imageId, bool isNew);

        CategoryViewModel ToCategoryViewModel(Category category);

        Task<Product> ToProductAsync(ProductViewModel model, bool isNew);

        ProductViewModel ToProductViewModel(Product product);


    }
}
