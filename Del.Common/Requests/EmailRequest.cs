using System.ComponentModel.DataAnnotations;

namespace Del.Common.Requests
{
    public class EmailRequest
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
