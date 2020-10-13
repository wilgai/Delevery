using Del.Common.Entities;
using Del.Common.Enums;
using Delevery.Web.Data.Entities;
using Delevery.Web.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Delevery.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly Random _random;

        public SeedDb(DataContext context, IUserHelper userHelper, IBlobHelper blobHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await CheckRolesAsync();
            await CheckUserAsync("1010", "Wilgai", "Lorimer", "lorimerwilgai23@outlook.com", "1 829 939 2939", "Santo Domingo Este", UserType.Admin);
            await CheckCategoriesAsync();
            await CheckRestaurantsAsync();
            await CheckProductsAsync();

        }


        private async Task CheckRestaurantsAsync()
        {
            if (!_context.Restaurants.Any())
            {
                User user = await _userHelper.GetUserAsync("lorimerwilgai23@outlook.com");
                string lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris gravida, nunc vel tristique cursus.";
                await AddRestaurantAsync("Burger King", "Ave. Venezuela", lorem, user);
                await AddRestaurantAsync("Mcdonald's", "Auto pista San Isidro", lorem, user);
                await AddRestaurantAsync("KFC", "Ave. Sabana Larga", lorem, user);
                await AddRestaurantAsync("Taco Bell", "Megacentro", lorem, user);
                await AddRestaurantAsync("Little Caesars", "Ave. Lope de vega", lorem, user);
                await _context.SaveChangesAsync();
            }
        }

        private async Task AddRestaurantAsync(string name, string address, string description, User user)
        {

            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images", $"{name}.png");
            Guid imageId = await _blobHelper.UploadBlobAsync(path, "restaurants");
            Restaurant restaurant = new Restaurant
            {
                Name = name,
                Address = address,
                ImageId = imageId,
                Qualifications = GetRandomQualifications(description, user)
            };
            _context.Restaurants.Add(restaurant);
        }

        private async Task CheckProductsAsync()
        {
            if (!_context.Products.Any())
            {
                User user = await _userHelper.GetUserAsync("lorimerwilgai23@outlook.com");

                Restaurant Burger_King = await _context.Restaurants.FirstOrDefaultAsync(r => r.Name == "Burger King");
                Restaurant Mcdonalds = await _context.Restaurants.FirstOrDefaultAsync(r => r.Name == "Mcdonald's");
                Restaurant KFC = await _context.Restaurants.FirstOrDefaultAsync(r => r.Name == "KFC");
                Restaurant Little_Caesars = await _context.Restaurants.FirstOrDefaultAsync(r => r.Name == "Little Caesars");
                Restaurant Taco_bell = await _context.Restaurants.FirstOrDefaultAsync(r => r.Name == "Taco Bell");
                Category Almuerzo = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "Almuerzo");
                Category Desayuno = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "Desayuno");
                Category Cena = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "Cena");
                string lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris gravida, nunc vel tristique cursus, velit nibh pulvinar enim, non pulvinar lorem leo eget felis. Proin suscipit dignissim nisl, at elementum justo laoreet sed. In tortor nibh, auctor quis est gravida.";
                await AddProductAsync(Almuerzo, lorem, Burger_King, "Whopper", 150, new string[] { "w1", "w2", "w3", "w4", "w5" }, user);
                await AddProductAsync(Desayuno, lorem, Mcdonalds, "Big Mac", 200, new string[] { "b1", "b2", "b3", "b4", "b5" }, user);
                await AddProductAsync(Almuerzo, lorem, KFC, "Wow Box", 250, new string[] { "k1", "k2", "k3", "k4", "k5" }, user);
                await AddProductAsync(Cena, lorem, Little_Caesars, "Pizza de Peperoni", 300, new string[] { "p1", "p2", "p3", "b1", "p5" }, user);
                await AddProductAsync(Almuerzo, lorem, Taco_bell, "Taco", 400, new string[] { "t1", "t2", "t3", "t4", "t5" }, user);
                await _context.SaveChangesAsync();
            }
        }

        private async Task AddProductAsync(Category category, string description, Restaurant restaurant, string name, decimal price, string[] images, User user)
        {
            Product product = new Product
            {
                Category = category,
                Description = description,
                Restaurant = restaurant,
                IsActive = true,
                Name = name,
                Price = price,
                ProductImages = new List<ProductImage>(),

            };

            foreach (string image in images)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images", $"{image}.png");
                Guid imageId = await _blobHelper.UploadBlobAsync(path, "products");
                product.ProductImages.Add(new ProductImage { ImageId = imageId });
            }

            _context.Products.Add(product);
        }

        private ICollection<Qualification> GetRandomQualifications(string description, User user)
        {
            List<Qualification> qualifications = new List<Qualification>();
            for (int i = 0; i < 10; i++)
            {
                qualifications.Add(new Qualification
                {
                    Date = DateTime.UtcNow,
                    Remarks = description,
                    Score = _random.Next(1, 5),
                    User = user
                });
            }

            return qualifications;
        }

        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                await AddCategoryAsync("Almuerzo");
                await AddCategoryAsync("Desayuno");
                await AddCategoryAsync("Cena");
                await _context.SaveChangesAsync();
            }
        }

        private async Task AddCategoryAsync(string name)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images", $"{name}.png");
            Guid imageId = await _blobHelper.UploadBlobAsync(path, "categories");
            _context.Categories.Add(new Category { Name = name, ImageId = imageId });
        }



        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task<User> CheckUserAsync(
            string document,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    UserType = userType
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            return user;
        }


    }


}
