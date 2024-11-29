using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TECH.Data.DatabaseEntity;

namespace TECH.Areas.Admin.Models
{
    public class PhongModelView
    {
        public int Id { get; set; }
        public int? MaNha { get; set; }
        public int? PhongTu { get; set; }
        public int? DenPhong { get; set; }
        public string? TenNha { get; set; }
        public string? TenPhong { get; set; }
        public decimal? DonGia { get; set; }
        public string? DonGiaStr { get; set; }
        public int? SLNguoiMax { get; set; }
        public float? ChieuDai { get; set; }
        public float? ChieuRong { get; set; }
        public string? MoTa { get; set; }
        public int? LoaiPhong { get; set; }
        public string? LoaiPhongStr { get; set; }
        public int? TinhTrang { get; set; }
        public string? TinhTrangStr { get; set; }
        public string? HinhAnh { get; set; }
        public int SoLuongNguoiDangO { get; set; }
        public decimal? SoDienCu { get; set; }
        public decimal? SoNuocCu { get; set; }

    }
    public class HomeModelViewCustomer
    {
        public Dictionary<string,List<PhongModelView>> PhongModelViews { get; set; }
        //public List<PhongModelView> PhongModelViews { get; set; }
        public List<DichVuModelView> DichVus { get; set; }
    }
}
