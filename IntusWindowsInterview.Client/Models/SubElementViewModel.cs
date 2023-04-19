using System.ComponentModel.DataAnnotations;

namespace IntusWindowsInterview.Client.Models
{
    public class SubElementViewModel
    {
        public long? Id { get; set; }
        public long WindowId { get; set; }
        [Required]
        [Display(Name = "Total Sub Elements")]
        [Range(0.0, int.MaxValue, ErrorMessage = "Element of SubElement must greater than 0")]
        public int Element { get; set; }
        [Required]
        [Display(Name = "SubElements Type")]
        public string Type { get; set; }
        [Required]
        [Display(Name = "SubElements Width")]
        [Range(0.0, int.MaxValue, ErrorMessage = "Width of SubElement must greater than 0")]
        public int Width { get; set; }
        [Required]
        [Display(Name = "SubElements Height")]
        [Range(0.0, int.MaxValue, ErrorMessage = "Height of SubElement must greater than 0")]
        public int Height { get; set; }
    }
}
