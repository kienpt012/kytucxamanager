
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Data.DatabaseEntity;
using TECH.Reponsitory;
using TECH.Utilities;

namespace TECH.Service
{
    public interface INhanVienService
    {
        PagedResult<NhanVienModelView> GetAllPaging(NhanVienViewModelSearch NhanVienModelViewSearch);
        NhanVienModelView GetByid(int id);
        void Add(NhanVienModelView view);
        bool Update(NhanVienModelView view);
        bool Deleted(int id);
        void Save();
        int GetCount();
        List<NhanVienModelView> GetAll();
        bool ChangePassWord(int userId, string current_password, string new_password, bool isCofirm = false);
        NhanVienModelView AppUserLogin(string userName, string passWord);
        bool UpdateDetailView(NhanVienModelView view);
        bool IsMailExist(string mail);
        bool IsPhoneExist(string phone);
    }

    public class NhanVienService : INhanVienService
    {
        private readonly INhanVienRepository _nhanVienRepository;
        private IUnitOfWork _unitOfWork;
        public NhanVienService(INhanVienRepository nhanVienRepository,
            IUnitOfWork unitOfWork)
        {
            _nhanVienRepository = nhanVienRepository;
            _unitOfWork = unitOfWork;
        }
        public List<NhanVienModelView> GetAll()
        {
            var data = _nhanVienRepository.FindAll().Select(n => new NhanVienModelView()
            {
                Id = n.Id,
                TenNV = n.TenNV,
                SoDienThoai = n.SoDienThoai,
                Email = n.Email,
                CMND = n.CMND,
                GioiTinh = n.GioiTinh,
                DiaChi = n.DiaChi,
                TenDangNhap = n.TenDangNhap,
                MatKhau = n.MatKhau,
                NgaySinh = n.NgaySinh,
                NgaySinhStr = n.NgaySinh.HasValue ? n.NgaySinh.Value.ToString("yyyy-MM-dd") : "",
                Role = n.Role,
                RoleStr = n.Role.HasValue && n.Role.Value == 0 ? "Quản trị viên" : "Nhân viên"
            }).ToList();
            if (data != null)
            {
                return data;
            }
            return null;
        }
        public bool IsMailExist(string mail)
        {
            var data = _nhanVienRepository.FindAll().Any(p => p.Email == mail);
            return data;
        }
        public bool IsPhoneExist(string phone)
        {
            var data = _nhanVienRepository.FindAll().Any(p => p.SoDienThoai == phone);
            return data;
        }
        public NhanVienModelView GetByid(int id)
        {
            var data = _nhanVienRepository.FindAll(p => p.Id == id).FirstOrDefault();
            if (data != null)
            {
                var model = new NhanVienModelView()
                {
                    Id = data.Id,
                    TenNV = data.TenNV,
                    SoDienThoai = data.SoDienThoai,
                    Email = data.Email,
                    CMND = data.CMND,
                    GioiTinh = data.GioiTinh,
                    DiaChi = data.DiaChi,
                    TenDangNhap = data.TenDangNhap,
                    MatKhau = data.MatKhau,
                    NgaySinh = data.NgaySinh,
                    NgaySinhStr = data.NgaySinh.HasValue ? data.NgaySinh.Value.ToString("yyyy-MM-dd") : "",
                    Role = data.Role,
                   RoleStr = data.Role.HasValue && data.Role.Value == 0 ? "Quản trị viên" : "Nhân viên"
                };
                return model;
            }
            return null;
        }
        public int GetCount()
        {
            int count = 0;
            count = _nhanVienRepository.FindAll().Count();
            return count;
        }
        public void Add(NhanVienModelView view)
        {
            try
            {
                if (view != null)
                {
                    var nhanvien = new NhanVien
                    {
                        TenNV = view.TenNV,
                        SoDienThoai = view.SoDienThoai,
                        Email = view.Email,
                        CMND = view.CMND,
                        GioiTinh = view.GioiTinh,
                        DiaChi = view.DiaChi,
                        TenDangNhap = view.TenDangNhap,
                        MatKhau = view.MatKhau,
                        NgaySinh = view.NgaySinh,
                        Role = view.Role
                    };
                    _nhanVienRepository.Add(nhanvien);
                }
            }
            catch (Exception ex)
            {
            }

        }
        public bool UpdateDetailView(NhanVienModelView view)
        {
            try
            {
                var dataServer = _nhanVienRepository.FindById(view.Id);
                if (dataServer != null)
                {
                    dataServer.DiaChi = view.DiaChi;
                    dataServer.TenNV = view.TenNV;
                    dataServer.Email = view.Email;
                    dataServer.SoDienThoai = view.SoDienThoai;
                    _nhanVienRepository.Update(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public bool Update(NhanVienModelView view)
        {
            try
            {
                var dataServer = _nhanVienRepository.FindById(view.Id);
                if (dataServer != null)
                {
                    dataServer.TenNV = view.TenNV;
                    dataServer.SoDienThoai = view.SoDienThoai;
                    dataServer.Email = view.Email;
                    dataServer.CMND = view.CMND;
                    dataServer.GioiTinh = view.GioiTinh;
                    dataServer.DiaChi = view.DiaChi;
                    dataServer.TenDangNhap = view.TenDangNhap;
                    dataServer.MatKhau = view.MatKhau;
                    dataServer.Role = view.Role;
                    dataServer.NgaySinh = view.NgaySinh;
                    _nhanVienRepository.Update(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }


        public bool ChangePassWord(int userId, string current_password, string new_password, bool isCofirm = false)
        {
            var modelUser = _nhanVienRepository.FindAll().Where(u => u.Id == userId).FirstOrDefault();
            bool status = false;
            if (modelUser != null)
            {
                if (modelUser.MatKhau == current_password)
                {
                    modelUser.MatKhau = new_password;
                    _nhanVienRepository.Update(modelUser);
                    status = true;
                }
                else
                {
                    status = false;
                }
            }
            else
            {
                status = false;
            }
            return status;
        }


        public NhanVienModelView AppUserLogin(string userName, string passWord)
        {
            var data = _nhanVienRepository.FindAll().Where(u => u.Email == userName || u.SoDienThoai == userName || u.TenDangNhap == userName).FirstOrDefault();
            if (data != null && data.MatKhau == passWord)
            {
                var model = GetByid(data.Id);
                if (model != null)
                {
                    return model;
                }
            }
            return null;
        }



        public bool Deleted(int id)
        {
            try
            {
                var dataServer = _nhanVienRepository.FindById(id);
                if (dataServer != null)
                {
                    _nhanVienRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public PagedResult<NhanVienModelView> GetAllPaging(NhanVienViewModelSearch NhanVienModelViewSearch)
        {
            try
            {
                var query = _nhanVienRepository.FindAll();

                if (!string.IsNullOrEmpty(NhanVienModelViewSearch.name))
                {
                    query = query.Where(c => c.TenNV.ToLower().Trim().Contains(NhanVienModelViewSearch.name.ToLower().Trim()) ||
                    c.Email.ToLower().Trim().Contains(NhanVienModelViewSearch.name.ToLower().Trim()) ||
                    c.SoDienThoai.ToLower().Trim().Contains(NhanVienModelViewSearch.name.ToLower().Trim()) ||
                    c.CMND.ToLower().Trim().Contains(NhanVienModelViewSearch.name.ToLower().Trim()) ||
                    c.DiaChi.ToLower().Trim().Contains(NhanVienModelViewSearch.name.ToLower().Trim()) ||
                    c.TenDangNhap.ToLower().Trim().Contains(NhanVienModelViewSearch.name.ToLower().Trim())
                    );
                }

                int totalRow = query.Count();
                query = query.Skip((NhanVienModelViewSearch.PageIndex - 1) * NhanVienModelViewSearch.PageSize).Take(NhanVienModelViewSearch.PageSize);
                var data = query.OrderByDescending(p => p.Id).Select(c => new NhanVienModelView()
                {
                    Id = c.Id,
                    TenNV = c.TenNV,
                    SoDienThoai = c.SoDienThoai,
                    Email = c.Email,
                    CMND = c.CMND,
                    GioiTinh = c.GioiTinh,
                    DiaChi = !string.IsNullOrEmpty(c.DiaChi) ? c.DiaChi : "",
                    TenDangNhap = !string.IsNullOrEmpty(c.TenDangNhap)? c.TenDangNhap:"",
                    MatKhau = c.MatKhau,
                    NgaySinh = c.NgaySinh,
                    NgaySinhStr = c.NgaySinh.HasValue ? c.NgaySinh.Value.ToString("dd/MM/yyyy"):"",
                    GioiTinhStr = !string.IsNullOrEmpty(c.GioiTinh)?(c.GioiTinh == "nam"?"Nam":"Nữ"):"",
                    Role = c.Role,
                    RoleStr = c.Role.HasValue && c.Role.Value == 0 ? "Quản trị viên" : "Nhân viên"
                }).ToList();

                var pagingData = new PagedResult<NhanVienModelView>
                {
                    Results = data,
                    CurrentPage = NhanVienModelViewSearch.PageIndex,
                    PageSize = NhanVienModelViewSearch.PageSize,
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
