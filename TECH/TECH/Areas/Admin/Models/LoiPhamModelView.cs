using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models
{
    public class LoiPhamModelView
    {
        public int Id { get; set; }
        public string? TenLoi { get; set; }
        public decimal? TienPhat { get; set; }
        public string? TienPhatStr { get; set; }
        public string? GhiChu { get; set; }
    }
}
