using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Del.Common.Entities;
using Delevery.Web.Data;
using Delevery.Web.Data.Entities;
using Delevery.Web.Helpers;
using Delevery.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;

namespace Delevery.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RestaurantsController : Controller
    {
       
            private readonly DataContext _context;

        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IFlashMessage _flashMessage;

        public RestaurantsController(DataContext context, IBlobHelper blobHelper, IConverterHelper converterHelper, IFlashMessage flashMessage)
        {
            _context = context;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
            _flashMessage = flashMessage;

        }


        public async Task<IActionResult> Index()
            {
                return View(await _context.Restaurants
                    .Include(p => p.Qualifications)
                    .ToListAsync());
            }

        public IActionResult Create()
        {
            RestaurantViewModel model = new RestaurantViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RestaurantViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "restaurants");
                }

                try
                {
                    Restaurant restaurant = _converterHelper.ToRestaurant(model, imageId, true);
                    _context.Add(restaurant);
                    await _context.SaveChangesAsync();
                    _flashMessage.Confirmation("Record was added");
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        //ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                        _flashMessage.Danger("There is already a record with same name.");
                    }
                    else
                    {
                        //ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                        _flashMessage.Danger("Error adding this record.");
                    }
                }
                catch (Exception exception)
                {
                    //ModelState.AddModelError(string.Empty, exception.Message);
                    _flashMessage.Danger("Error adding this record.");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Restaurant category = await _context.Restaurants.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            RestaurantViewModel model = _converterHelper.ToRestaurantViewModel(category);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RestaurantViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = model.ImageId;

                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "restaurants");
                }

                try
                {
                    Restaurant restaurant = _converterHelper.ToRestaurant(model, imageId, false);
                    _context.Update(restaurant);
                    await _context.SaveChangesAsync();
                    _flashMessage.Confirmation("Record was added");
                    return RedirectToAction(nameof(Index));

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        //ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                        _flashMessage.Danger("There are a record with the same name.");
                    }
                    else
                    {
                        //ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                        _flashMessage.Danger("Error editing this record.");

                    }
                }
                catch (Exception exception)
                {
                    //ModelState.AddModelError(string.Empty, exception.Message);
                    _flashMessage.Danger("Error editing this record.");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Restaurant product = await _context.Restaurants
                .Include(c => c.Qualifications)
                .ThenInclude(q => q.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Restaurant restaurant = await _context.Restaurants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (restaurant == null)
            {
                return NotFound();
            }

            try
            {
                _context.Restaurants.Remove(restaurant);
                _flashMessage.Confirmation("Record was deleted.");
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError(string.Empty, ex.Message);
                _flashMessage.Danger("Error deleting this record.");
            }

            return RedirectToAction(nameof(Index));
        }




    }
}
