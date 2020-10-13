using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Del.Common.Requests
{
    public class QualificationRequest
    {
        [Required]
        public int RestaurantId { get; set; }

        [Range(0, 5)]
        [Required]
        public float Score { get; set; }

        public string Remarks { get; set; }
    }

}
