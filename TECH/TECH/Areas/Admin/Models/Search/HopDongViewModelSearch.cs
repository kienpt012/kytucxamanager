using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models.Search
{
    public class HopDongViewModelSearch : PageViewModel
    {
        public string? name { get; set; }
        public int? maKH { get; set; }
        //public int differentiate { get; set; }
        public int? status { get; set; }
        
    }
}
