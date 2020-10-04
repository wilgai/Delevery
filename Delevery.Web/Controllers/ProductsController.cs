using Del.Common.Entities;
using Delevery.Web.Data;
using Delevery.Web.Helpers;
using Delevery.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onsale.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly DataContext _context;
        private readonly IBlobHelper _blobHelper;
        private readonly ICategoryCombosHelper _categorycombosHelper;
        private readonly ICategoryConverterHelper _categoryConverterHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IRestaurantCombosHelper _restaurantCombosHelper;

        public ProductsController(DataContext context, IBlobHelper blobHelper, ICategoryCombosHelper categorycombosHelper, IConverterHelper converterHelper,
            ICategoryConverterHelper categoryConverterHelper, IRestaurantCombosHelper restaurantCombosHelper)
        {
            _context = context;
            _blobHelper = blobHelper;
            _categorycombosHelper = categorycombosHelper;
            _converterHelper = converterHelper;
            _categoryConverterHelper = categoryConverterHelper;
            _restaurantCombosHelper = restaurantCombosHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .ToListAsync());
        }
        public IActionResult Create()
        {
            ProductViewModel model = new ProductViewModel
            {
                Categories = _categorycombosHelper.GetComboCategories(),
                Restaurants= _restaurantCombosHelper.GetComboRestaurants(),
                IsActive = true
            };

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Product product = await _categoryConverterHelper.ToProductAsync(model, true);

                    if (model.ImageFile != null)
                    {
                        Guid imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "products");
                        product.ProductImages = new List<ProductImage>
                {
                    new ProductImage { ImageId = imageId }
                };
                    }

                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            model.Categories = _categorycombosHelper.GetComboCategories();
            model.Restaurants = _restaurantCombosHelper.GetComboRestaurants();
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.Restaurant)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            ProductViewModel model = _categoryConverterHelper.ToProductViewModel(product);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Product product = await _categoryConverterHelper.ToProductAsync(model, false);

                    if (model.ImageFile != null)
                    {
                        Guid imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "products");
                        if (product.ProductImages == null)
                        {
                            product.ProductImages = new List<ProductImage>();
                        }

                        product.ProductImages.Add(new ProductImage { ImageId = imageId });
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            model.Categories = _categorycombosHelper.GetComboCategories();
            model.Restaurants = _restaurantCombosHelper.GetComboRestaurants();
            return View(model);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Restaurant)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            try
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await _context.Products
                .Include(c => c.Category)
                .Include(c => c.Restaurant)
                .Include(c => c.ProductImages)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        public async Task<IActionResult> AddImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            AddProductImageViewModel model = new AddProductImageViewModel { ProductId = product.Id };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage(AddProductImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                Product product = await _context.Products
                    .Include(p => p.ProductImages)
                    .FirstOrDefaultAsync(p => p.Id == model.ProductId);
                if (product == null)
                {
                    return NotFound();
                }

                try
                {
                    Guid imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "products");
                    if (product.ProductImages == null)
                    {
                        product.ProductImages = new List<ProductImage>();
                    }

                    product.ProductImages.Add(new ProductImage { ImageId = imageId });
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction($"{nameof(Details)}/{product.Id}");

                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return View(model);
        }


        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductImage productImage = await _context.ProductImages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productImage == null)
            {
                return NotFound();
            }

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.ProductImages.FirstOrDefault(pi => pi.Id == productImage.Id) != null);
            _context.ProductImages.Remove(productImage);
            await _context.SaveChangesAsync();
            return RedirectToAction($"{nameof(Details)}/{product.Id}");
        }



    }

}
