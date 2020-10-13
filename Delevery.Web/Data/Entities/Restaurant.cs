using Microsoft.VisualStudio.Core.Imaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Delevery.Web.Data.Entities
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

        public ICollection<Qualification> Qualifications { get; set; }

        [DisplayName("Product Qualifications")]
        public int RestaurantQualifications => Qualifications == null ? 0 : Qualifications.Count;

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public float Qualification => Qualifications == null || Qualifications.Count == 0 ? 0 : Qualifications.Average(q => q.Score);


    }
}
