using System.ComponentModel.DataAnnotations;

namespace IntusWindowsInterview.Client.Models
{
    public class OrderViewModel
    {
        public long Id { get; set; }
        [Required]
        [Display(Name = "Order Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Order State")]
        [MaxLength(2, ErrorMessage = "Maximum 2 characters")]
        public string State { get; set; }

        public List<WindowViewModel>? WindowsViewModels { get; set; } = new List<WindowViewModel>();
    }
}
