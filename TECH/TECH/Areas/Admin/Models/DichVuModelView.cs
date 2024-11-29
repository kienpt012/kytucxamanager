using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models
{
    public class DichVuModelView
    {
        public int Id { get; set; }
        public string? TenDV { get; set; }
        public decimal? DonGia { get; set; }
        public string? DonGiaStr { get; set; }
        public string? GhiChu { get; set; }
        public int? LoaiDV { get; set; }
        public string? LoaiDVStr { get; set; }
        public int? SoLuong { get; set; }   
    }
}
