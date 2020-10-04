

using System;
using System.Threading.Tasks;
using Delevery.Web.Helpers;
using Del.Common.Entities;
using Delevery.Web.Models;
using Delevery.Web.Data;

namespace Onsale.Web.Helpers
{
    public class CategoryConverterHelper : ICategoryConverterHelper
    {
        private readonly DataContext _context;
        private readonly ICategoryCombosHelper _categorycombosHelper;
        private readonly IRestaurantCombosHelper _restaurantCombosHelper;
        public CategoryConverterHelper(DataContext context, ICategoryCombosHelper categorycombosHelper, IRestaurantCombosHelper restaurantCombosHelper)
        {
            this._context = context;
            this._categorycombosHelper = categorycombosHelper;
            _restaurantCombosHelper = restaurantCombosHelper;
        }
        public Category ToCategory(CategoryViewModel model, Guid imageId, bool isNew)
        {
            return new Category
            {
                Id = isNew ? 0 : model.Id,
                ImageId = imageId,
                Name = model.Name
            };
        }

        

        public CategoryViewModel ToCategoryViewModel(Category category)
        {
            return new CategoryViewModel
            {
                Id = category.Id,
                ImageId = category.ImageId,
                Name = category.Name
            };
        }

        

        public async Task<Product> ToProductAsync(ProductViewModel model, bool isNew)
        {
            return new Product
            {
                Category = await _context.Categories.FindAsync(model.CategoryId),
                Restaurant = await _context.Restaurants.FindAsync(model.RestaurantId),
                Description = model.Description,
                Id = isNew ? 0 : model.Id,
                IsActive = model.IsActive,
                IsStarred = model.IsStarred,
                Name = model.Name,
                Price = model.Price,
                ProductImages = model.ProductImages
            };
        }

       

        public ProductViewModel ToProductViewModel(Product product)
        {
            return new ProductViewModel
            {
                Restaurants = _restaurantCombosHelper.GetComboRestaurants(),
                Restaurant=product.Restaurant,
                RestaurantId = product.Restaurant.Id,
                Categories = _categorycombosHelper.GetComboCategories(),
                Category = product.Category,
                CategoryId = product.Category.Id,
                Description = product.Description,
                Id = product.Id,
                IsActive = product.IsActive,
                IsStarred = product.IsStarred,
                Name = product.Name,
                Price = product.Price,
                ProductImages = product.ProductImages


            };
        }

      
    }
}
