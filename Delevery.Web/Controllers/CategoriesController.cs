﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Del.Common.Entities;
using Delevery.Web.Data;
using Delevery.Web.Helpers;
using Delevery.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;

namespace Delevery.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly DataContext _context;
        private readonly IBlobHelper _blobHelper;
        private readonly ICategoryConverterHelper _categoryconverterHelper;
        private readonly IFlashMessage _flashMessage;

        public CategoriesController(DataContext context, IBlobHelper blobHelper, ICategoryConverterHelper categoryconverterHelper, IFlashMessage flashMessage)
        {
            _context = context;
            _blobHelper = blobHelper;
            _categoryconverterHelper = categoryconverterHelper;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        public IActionResult Create()
        {
            CategoryViewModel model = new CategoryViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "categories");
                }

                try
                {
                    Category category = _categoryconverterHelper.ToCategory(model, imageId, true);
                    _context.Add(category);
                    await _context.SaveChangesAsync();
                    _flashMessage.Confirmation("Category was added");
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
                        _flashMessage.Danger("Sorry, the record can't be added");
                    }
                }
                catch (Exception exception)
                {
                    //ModelState.AddModelError(string.Empty, exception.Message);
                    _flashMessage.Danger("Sorry, the record can't be added");
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

            Category category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            CategoryViewModel model = _categoryconverterHelper.ToCategoryViewModel(category);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = model.ImageId;

                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "categories");
                }

                try
                {
                    Category category = _categoryconverterHelper.ToCategory(model, imageId, false);
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    _flashMessage.Confirmation("The category was edited.");
                    return RedirectToAction(nameof(Index));
                    

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        //ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                        _flashMessage.Danger("There is a record with the same name.");
                    }
                    else
                    {
                        //ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                        _flashMessage.Danger("The category can't be edited.");
                    }
                }
                catch (Exception exception)
                {
                    //ModelState.AddModelError(string.Empty, exception.Message);
                    _flashMessage.Danger("The category can't be edited.");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            try
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                _flashMessage.Confirmation("The category was deleted.");
            }
            catch (Exception ex)
            {
               
                _flashMessage.Danger("The category can't be deleted because it has related records.");
            }

            return RedirectToAction(nameof(Index));
        }



    }


}
