using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TECH.Data.DatabaseEntity;

namespace TECH.Areas.Admin.Models
{
    public class NhaModelView
    {
        public int Id { get; set; }
        public string? TenNha { get; set; }
        public int? MaTP { get; set; }
        public string? MaTPStr { get; set; }
        public int? MaQH { get; set; }
        public string? MaQHStr { get; set; }
        public int? MaPX { get; set; }
        public string? MaPXStr { get; set; }
        //public int? SLPhong { get; set; }
        public string? DiaChi { get; set; }
        public List<PhongModelView> Phongs { get; set; }
    }
}
