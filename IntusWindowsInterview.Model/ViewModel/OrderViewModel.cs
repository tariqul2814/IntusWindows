using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IntusWindowsInterview.Model.ViewModel
{
    public class OrderViewModel
    {
        public long? Id { get; set; }
        [Required]
        [Display(Name = "Order Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Order State")]
        [MaxLength(2, ErrorMessage = "Maximum 2 characters")]
        public string State { get; set; }

        public List<WindowViewModel>? WindowsViewModels { get; set; }
    }
}
