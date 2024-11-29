using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TECH.Data.DatabaseEntity;

namespace TECH.Areas.Admin.Models
{
    public class TheKiTucXaModelView
    {
        public int Id { get; set; }
        public int? MaSinhVien { get; set; }
        public KhachHang? KhachHang { get; set; }
        public int? Status { get; set; }
        public string? StatusSrc { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public string? NgayBatDauSrc { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public string? NgayKetThucSrc { get; set; }
        public string? MaThe { get; set; }
    }
   
}
