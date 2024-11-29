
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Data.DatabaseEntity;
using TECH.General;
using TECH.Reponsitory;
using TECH.Utilities;

namespace TECH.Service
{
    public interface IHopDongService
    {
        PagedResult<HopDongModelView> GetAllPaging(HopDongViewModelSearch HopDongModelViewSearch);
        HopDongModelView GetByid(int id);
        //HopDongModelView GetObjectHopDongByid(int id);
        //List<HopDongModelView> GetPhongByNha(int id);
        //void AddPhongFast(HopDongModelView view);
        void Add(HopDongModelView view);
        List<HopDongModelView> GetHopDongByPhong(List<int> phongs);
        List<HopDongModelView> GetPhongByHopDong(); 
        bool Update(HopDongModelView view);
        void UpdateIsDeteled(int id, bool status);
        bool Deleted(int id);
        void Save();
        int GetCount();

        HopDongModelView GetHopDongByPhong(int maPhong);
        HopDongModelView GetHopDongByKhachHangAndPhong(int maKhachHang,int maPhong);
    }

    public class HopDongService : IHopDongService
    {
        private readonly IHopDongRepository _hopDongRepository;
        private readonly INhaRepository _nhaRepository;
        private readonly IPhongRepository _phongRepository;
        private readonly IKhachHangRepository _khachHangRepository;
        private IUnitOfWork _unitOfWork;
        public HopDongService(IHopDongRepository hopDongRepository,
            INhaRepository nhaRepository,
            IPhongRepository phongRepository,
            IKhachHangRepository khachHangRepository,
        IUnitOfWork unitOfWork)
        {
            _hopDongRepository = hopDongRepository;
            _nhaRepository = nhaRepository;
            _phongRepository = phongRepository;
            _khachHangRepository = khachHangRepository;
            _unitOfWork = unitOfWork;
        }   
        //public HopDongModelView GetObjectHopDongByid(int id)
        //{
        //    var data = _hopDongRepository.FindAll(p => p.TrangThai == 1 && p.Id == id).Select(p => new HopDongModelView()
        //    {
        //        Id = p.Id,
        //        MaPhong = p.MaPhong,
        //        MaNV = p.MaNV,
        //        MaKH = p.MaKH,
        //        MaNha = p.MaNha,
        //        NgayBatDau = p.NgayBatDau,
        //        NgayKetThuc = p.NgayKetThuc,
        //        TienCoc = p.TienCoc,
        //        TrangThai = p.TrangThai,
        //    }).FirstOrDefault();
        //    if (data != null)
        //    {
        //        var phong = _phongRepository.h
        //    }
        //}

        /// <summary>
        /// lấy tất cả phòng có hợp đồng và đang được thuê
        /// </summary>
        /// <returns></returns>
        public List<HopDongModelView> GetPhongByHopDong()
        {
            var data = _hopDongRepository.FindAll(p => p.TrangThai == 1 &&p.IsDeteled !=true).Select(p => new HopDongModelView()
            {
                Id = p.Id,
                MaPhong = p.MaPhong,
                MaNV = p.MaNV,
                MaKH = p.MaKH,
                MaNha = p.MaNha,
                NgayBatDau = p.NgayBatDau,
                NgayKetThuc = p.NgayKetThuc,
                TienCoc = p.TienCoc,
                TrangThai = p.TrangThai,
                IsDeteled = p.IsDeteled,
                GhiChu = p.GhiChu
            }).ToList();
            return data;
        }
        public List<HopDongModelView> GetHopDongByPhong(List<int> phongs)
        {
            if (phongs != null && phongs.Count > 0)
            {
                var data = _hopDongRepository.FindAll(p => phongs.Contains(p.MaPhong.Value) && p.TrangThai == 1).Select(p=> new HopDongModelView()
                {
                    Id = p.Id,
                    MaPhong = p.MaPhong,
                    MaNV = p.MaNV,
                    MaKH = p.MaKH,
                    MaNha = p.MaNha,
                    NgayBatDau = p.NgayBatDau,
                    NgayKetThuc = p.NgayKetThuc,
                    TienCoc = p.TienCoc,
                    TrangThai = p.TrangThai,
                    IsDeteled = p.IsDeteled,
                    GhiChu = p.GhiChu
                }).ToList();
                return data;
            }
            return null;
        }
       public HopDongModelView GetHopDongByKhachHangAndPhong(int maKhachHang,int maPhong)
        {
            if (maPhong > 0 && maKhachHang > 0)
            {
                var data = _hopDongRepository.FindAll().Where(h => h.MaPhong == maPhong && h.MaKH == maKhachHang && h.IsDeteled !=true).Select(p => new HopDongModelView()
                {
                    Id = p.Id,
                    MaPhong = p.MaPhong,
                    MaNV = p.MaNV,
                    MaKH = p.MaKH,
                    MaNha = p.MaNha,
                    NgayBatDau = p.NgayBatDau,
                    NgayKetThuc = p.NgayKetThuc,
                    TienCoc = p.TienCoc,
                    TrangThai = p.TrangThai,
                    IsDeteled = p.IsDeteled,
                    GhiChu = p.GhiChu
                }).FirstOrDefault();
                return data;
            }
            return null;
        }
        public HopDongModelView GetHopDongByPhong(int maPhong)
        {
            if (maPhong > 0)
            {
                var data = _hopDongRepository.FindAll(p => p.MaPhong.Value == maPhong && p.TrangThai == 1).Select(p => new HopDongModelView()
                {
                    Id = p.Id,
                    MaPhong = p.MaPhong,
                    MaNV = p.MaNV,
                    MaKH = p.MaKH,
                    MaNha = p.MaNha,
                    NgayBatDau = p.NgayBatDau,
                    NgayKetThuc = p.NgayKetThuc,
                    TienCoc = p.TienCoc,
                    TrangThai = p.TrangThai,
                    IsDeteled = p.IsDeteled,
                    GhiChu = p.GhiChu
                }).FirstOrDefault();
                return data;
            }
            return null;
        }
        public HopDongModelView GetByid(int id)
        {
            var data = _hopDongRepository.FindAll(p => p.Id == id && p.IsDeteled !=true).FirstOrDefault();
            if (data != null)
            {
                var model = new HopDongModelView()
                {
                    Id = data.Id,
                    MaPhong = data.MaPhong,
                    MaNV = data.MaNV,
                    MaKH = data.MaKH,
                    MaNha = data.MaNha,
                    NgayBatDau = data.NgayBatDau,
                    NgayKetThuc = data.NgayKetThuc,
                    TienCoc = data.TienCoc,
                    GhiChu =data.GhiChu,
                    TrangThai = data.TrangThai,
                    IsDeteled = data.IsDeteled,
                    NgayBatDauStr = data.NgayBatDau.HasValue ? data.NgayBatDau.Value.ToString("dd/MM/yyyy") : "",
                    NgayKetThucStr = data.NgayKetThuc.HasValue ? data.NgayKetThuc.Value.ToString("dd/MM/yyyy") : ""
                };
                return model;
            }
            return null;
        }
        //public List<HopDongModelView> GetPhongByNha(int id)
        //{
        //    if (id > 0)
        //    {
        //        var data = _hopDongRepository.FindAll(p => p.Id == id).Select(c => new HopDongModelView()
        //        {
        //            Id = c.Id,
        //            MaNha = c.MaNha,
        //            TenPhong = c.TenPhong,
        //            DonGia = c.DonGia,
        //            SLNguoiMax = c.SLNguoiMax,
        //            ChieuDai = c.ChieuDai,
        //            ChieuRong = c.ChieuRong,
        //            MoTa = c.MoTa,
        //            LoaiPhong = c.LoaiPhong,
        //            TinhTrang = c.TinhTrang,
        //            TinhTrangStr = Common.GetTinhTrangPhong(c.TinhTrang.Value)
        //        }).ToList();

        //        return data;
        //    }
        //    return null;
        //}
        public int GetCount()
        {
            int count = 0;
            count = _hopDongRepository.FindAll().Count();
            return count;
        }
        public void Add(HopDongModelView view)
        {
            try
            {
                if (view != null)
                {
                    var hopdong = new HopDong
                    {
                        MaPhong = view.MaPhong,
                        MaNV = view.MaNV,
                        MaKH = view.MaKH,
                        MaNha = view.MaNha,
                        NgayBatDau = view.NgayBatDau,
                        NgayKetThuc = view.NgayKetThuc,
                        TienCoc = view.TienCoc,
                        GhiChu = view.GhiChu,                        
                        TrangThai = view.TrangThai,
                    };
                    _hopDongRepository.Add(hopdong);
                }
            }
            catch (Exception ex)
            {
            }
        }
        //public void AddPhongFast(HopDongModelView view)
        //{
        //    if (view != null && view.PhongTu > 0 && view.DenPhong > 0 && view.PhongTu < view.DenPhong)
        //    {
        //        for (int i = view.PhongTu.Value; i <= view.DenPhong.Value; i++)
        //        {
        //            view.TenPhong = i.ToString();
        //            Add(view);
        //        }
        //        //Save();
        //    }
        //}
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public bool Update(HopDongModelView view)
        {
            try
            {
                var dataServer = _hopDongRepository.FindById(view.Id);
                if (dataServer != null)
                {
                    dataServer.MaPhong = view.MaPhong;
                    dataServer.MaNV = view.MaNV;
                    dataServer.MaKH = view.MaKH;
                    dataServer.MaNha = view.MaNha;
                    dataServer.NgayBatDau = view.NgayBatDau;
                    dataServer.NgayKetThuc = view.NgayKetThuc;
                    dataServer.TienCoc = view.TienCoc;
                    dataServer.TrangThai = view.TrangThai;
                    dataServer.GhiChu = view.GhiChu;
                    _hopDongRepository.Update(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }      
        public bool Deleted(int id)
        {
            try
            {
                var dataServer = _hopDongRepository.FindById(id);
                if (dataServer != null)
                {
                    _hopDongRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public void UpdateIsDeteled(int id, bool status)
        {
            var dataServer = _hopDongRepository.FindById(id);
            if (dataServer != null)
            {
                dataServer.IsDeteled = status;
                _hopDongRepository.Update(dataServer);
                //Save();
            }

        }
        public PagedResult<HopDongModelView> GetAllPaging(HopDongViewModelSearch HopDongModelViewSearch)
        {
            try
            {
                var query = _hopDongRepository.FindAll();

                if (HopDongModelViewSearch.maKH.HasValue && HopDongModelViewSearch.maKH.Value > 0)
                {
                    query = query.Where(c => c.MaKH == HopDongModelViewSearch.maKH.Value);
                }
                //if (!string.IsNullOrEmpty(HopDongModelViewSearch.name))
                //{
                //    query = query.Where(c => c.TenPhong.ToLower().Trim().Contains(HopDongModelViewSearch.name.ToLower().Trim()));
                //}

                int totalRow = query.Count();
                query = query.Skip((HopDongModelViewSearch.PageIndex - 1) * HopDongModelViewSearch.PageSize).Take(HopDongModelViewSearch.PageSize);
                var data = query.OrderByDescending(p => !p.IsDeteled).Select(c => new HopDongModelView()
                {
                    Id = c.Id,
                    MaPhong =c.MaPhong,
                    MaNV=c.MaNV,
                    MaKH=c.MaKH,
                    MaNha=c.MaNha,
                    NgayBatDau=c.NgayBatDau,
                    NgayKetThuc=c.NgayKetThuc,
                    TienCoc=c.TienCoc,
                    TrangThai=c.TrangThai,
                    IsDeteled = c.IsDeteled,
                    NgayBatDauStr = c.NgayBatDau.HasValue ? c.NgayBatDau.Value.ToString("dd/MM/yyyy") : "",
                    NgayKetThucStr = c.NgayKetThuc.HasValue ? c.NgayKetThuc.Value.ToString("dd/MM/yyyy") : ""
                }).ToList();

                var pagingData = new PagedResult<HopDongModelView>
                {
                    Results = data,
                    CurrentPage = HopDongModelViewSearch.PageIndex,
                    PageSize = HopDongModelViewSearch.PageSize,
                    RowCount = totalRow,
                };
                return pagingData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
