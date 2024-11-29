using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TECH.Data.DatabaseEntity;

namespace TECH.Areas.Admin.Models
{
    public class ChiTietHoaDonModelView
    {
        public int Id { get; set; }
        public int? MaHoaDon { get; set; }
        public HoaDonModelView? HoaDon { get; set; }
        public PhongModelView? Phong { get; set; }
        public int? MaDV { get; set; }

        public int? MaLoi { get; set; }
        public LoiPhamModelView? LoiPhams { get; set; }
        public DichVuModelView? DichVu { get; set; }
        public List<DichVuPhongModelView>? DichVus { get; set; }

        public int? ChiSoCu => Phong != null
     ? (int?)(Phong.SoDienCu + Phong.SoNuocCu)
     : null;

        public int? ChiSoMoi { get; set; }
        public int? ChiSoDung { get; set; }
        public decimal? ThanhTien { get; set; }
        public string? ThanhTienStr { get; set; }
    }


    public class ChiTietHoaDonIndexModelViews
    {
        public HoaDonModelView? HoaDon { get; set; }
        public PhongModelView? Phong { get; set; }
        public List<ChiTietHoaDonModelView> ChiTietHoaDonModelViews { get; set; }
        public List<LoiPhamModelView> LoiPhamModelViews { get; set; }
        public decimal TongTien { get; set; }

        // Added new properties for meter readings
        public int? SoDienCu { get; set; }
        public int? SoNuocCu { get; set; }
        public int? SoDienMoi { get; set; }
        public int? SoNuocMoi { get; set; }

        // Constructor to initialize lists
        public ChiTietHoaDonIndexModelViews()
        {
            ChiTietHoaDonModelViews = new List<ChiTietHoaDonModelView>();
            LoiPhamModelViews = new List<LoiPhamModelView>();
        }
    }

}
