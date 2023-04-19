using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace IntusWindowsInterview.Model.DBModel
{
    public class Window
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long OrderId { get; set; }
        [Required]
        [Display(Name = "Window Name")]
        public string Name { get; set; }
        [Required]
        [Display (Name = "Quantity of Windows")]
        [Range(0.0, int.MaxValue, ErrorMessage ="Quantity of Windows must greater than 0")]
        public int QuantityOfWindows { get; set; }
        [Required]
        [Display(Name = "Total Sub Elements")]
        [Range(0.0, int.MaxValue, ErrorMessage = "Total Sub Elements must greater than 0")]
        public int TotalSubElements { get; set; }

        public Order Order { get; set; }
        public List<SubElement> SubElements { get; set; } 
    }
}
