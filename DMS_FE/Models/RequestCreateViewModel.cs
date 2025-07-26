using System.ComponentModel.DataAnnotations;

namespace DMS_FE.Models
{
    public class RequestCreateViewModel
    {
        [Required]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Request type is required")]
        [Display(Name = "Request Type")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;
    }
} 