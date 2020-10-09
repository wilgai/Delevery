using Del.Common.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Delevery.Web.Data.Entities
{
    public class User : IdentityUser
    {
        [MaxLength(20)]
        
        public string Document { get; set; }

        [Display(Name = "First Name")]
        [MaxLength(50)]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(50)]
        [Required]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string Address { get; set; }

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        //TODO: Pending to put the correct paths

         [Display(Name = "Login Type")]
         public LoginType LoginType { get; set; }

         public string ImageFacebook { get; set; }

         [Display(Name = "Image")]
         public string ImageFullPath
         {
             get
             {
                 if (LoginType == LoginType.Facebook && string.IsNullOrEmpty(ImageFacebook) ||
                     LoginType == LoginType.OnSale && ImageId == Guid.Empty)
                 {
                     return $"https://delevery.azurewebsites.net/images/noimage.png";
                 }

                 if (LoginType == LoginType.Facebook)
                 {
                     return ImageFacebook;
                 }

                 return $"https://delevery.blob.core.windows.net/users/{ImageId}";
             }
         }
        /*[Display(Name = "Image")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://delevery.azurewebsites.net/images/noimage.png"
            : $"https://delevery.blob.core.windows.net/users/{ImageId}";*/


        [Display(Name = "User Type")]
        public UserType UserType { get; set; }

        [Display(Name = "User")]
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "User")]
        public string FullNameWithDocument => $"{FirstName} {LastName} - {Document}";
    }

}
