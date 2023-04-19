using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IntusWindowsInterview.Model.ViewModel
{
    public class OrderSerializeViewModel
    {
        public long OrderId { get; set; }
        public string OrderName { get; set; }
        public string State { get; set; }
        public long WindowId { get; set; }
        public string WindowName { get; set; }
        public int QuantityOfWindows { get; set; }
        public int TotalSubElements { get; set; }
        public long SubElementId { get; set; }
        public int Element { get; set; }
        public string Type { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
