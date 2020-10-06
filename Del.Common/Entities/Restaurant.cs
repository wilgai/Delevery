using Microsoft.VisualStudio.Core.Imaging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Del.Common.Entities
{
    public class Restaurant
    {
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        [MaxLength(100)]
        [Required]
        public string Address { get; set; }
        public Guid ImageId { get; set; }

        //TODO: Pending to put the correct paths
        [Display(Name = "Image")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://delevery.azurewebsites.net/images/noimage.png"
            : $"https://delevery.blob.core.windows.net/restaurants/{ImageId}";
    }
}
