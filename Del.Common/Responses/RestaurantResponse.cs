using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Del.Common.Responses
{
    public class RestaurantResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public Guid ImageId { get; set; }

       

        [Display(Name = "Image")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://delevery.azurewebsites.net/images/noimage.png"
            : $"https://delevery.blob.core.windows.net/restaurants/{ImageId}";

        public ICollection<QualificationResponse> Qualifications { get; set; }

        public int ProductQualifications => Qualifications == null ? 0 : Qualifications.Count;

        public float Qualification => Qualifications == null || Qualifications.Count == 0 ? 0 : Qualifications.Average(q => q.Score);
    }


}
