
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
    public interface INhaService
    {
        PagedResult<NhaModelView> GetAllPaging(NhaViewModelSearch NhaModelViewSearch);
        NhaModelView GetByid(int id);
        List<NhaModelView> GetAll();
        void Add(NhaModelView view);
        bool Update(NhaModelView view);
        bool Deleted(int id);
        void Save();
        int GetCount();
        List<NhaModelView> GetAllMenu();
        bool IsExist(string tenNhan);
    }

    public class NhaService : INhaService
    {
        private readonly INhaRepository _nhaRepository;
        private IUnitOfWork _unitOfWork;
        public NhaService(INhaRepository nhaRepository,
            IUnitOfWork unitOfWork)
        {
            _nhaRepository = nhaRepository;
            _unitOfWork = unitOfWork;
        }       
        public NhaModelView GetByid(int id)
        {
            var data = _nhaRepository.FindAll(p => p.Id == id).FirstOrDefault();
            if (data != null)
            {
                var model = new NhaModelView()
                {
                    Id = data.Id,
                    TenNha = data.TenNha,
                    MaTP = data.MaTP,
                    MaQH = data.MaQH,
                    MaPX = data.MaPX,
                    DiaChi = data.DiaChi
                };
                return model;
            }
            return null;
        }
        public bool IsExist(string tenNhan)
        {
            var data = _nhaRepository.FindAll().Any(p => p.TenNha == tenNhan);
            return data;
        }
        public int GetCount()
        {
            int count = 0;
            count = _nhaRepository.FindAll().Count();
            return count;
        }
        public void Add(NhaModelView view)
        {
            try
            {
                if (view != null)
                {
                    var nha = new Nha
                    {
                        TenNha = view.TenNha,
                        MaTP = view.MaTP,
                        MaQH = view.MaQH,
                        MaPX = view.MaPX,
                        DiaChi = view.DiaChi
                    };
                    _nhaRepository.Add(nha);
                }
            }
            catch (Exception ex)
            {
            }

        }
        public List<NhaModelView> GetAll()
        {
            var data = _nhaRepository.FindAll().Select(n=> new NhaModelView()
            {
                Id=n.Id,
                TenNha = n.TenNha
            }).ToList();
            if (data != null)
            {
                return data;
            }
            return null;
        }
        public List<NhaModelView> GetAllMenu()
        {
            var data = _nhaRepository.FindAll().Select(c => new NhaModelView()
            {
                Id = c.Id,
                TenNha = c.TenNha
            }).ToList();

            return data;
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public bool Update(NhaModelView view)
        {
            try
            {
                var dataServer = _nhaRepository.FindById(view.Id);
                if (dataServer != null)
                {
                    if (dataServer.TenNha.ToLower().Trim() != view.TenNha.ToLower().Trim())
                    {
                        if (IsExist(view.TenNha))
                        {
                            return false;
                        }
                    }
                    dataServer.TenNha = view.TenNha;
                    dataServer.MaTP = view.MaTP;
                    dataServer.MaQH = view.MaQH;
                    dataServer.MaPX = view.MaPX;
                    dataServer.DiaChi = view.DiaChi;
                    _nhaRepository.Update(dataServer);
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
                var dataServer = _nhaRepository.FindById(id);
                if (dataServer != null)
                {
                    _nhaRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public PagedResult<NhaModelView> GetAllPaging(NhaViewModelSearch NhaModelViewSearch)
        {
            try
            {
                var query = _nhaRepository.FindAll();

                if (!string.IsNullOrEmpty(NhaModelViewSearch.name))
                {
                    query = query.Where(c => c.TenNha.ToLower().Trim().Contains(NhaModelViewSearch.name.ToLower().Trim()));
                }

                int totalRow = query.Count();
                query = query.Skip((NhaModelViewSearch.PageIndex - 1) * NhaModelViewSearch.PageSize).Take(NhaModelViewSearch.PageSize);
                var data = query.OrderByDescending(p => p.Id).Select(c => new NhaModelView()
                {
                    Id = c.Id,
                    TenNha = !string.IsNullOrEmpty(c.TenNha) ? c.TenNha : "",
                    DiaChi = !string.IsNullOrEmpty(c.DiaChi) ? c.DiaChi : "",
                    MaTP = c.MaTP,
                    MaQH = c.MaQH,
                    MaPX = c.MaPX                    
                }).ToList();

                var pagingData = new PagedResult<NhaModelView>
                {
                    Results = data,
                    CurrentPage = NhaModelViewSearch.PageIndex,
                    PageSize = NhaModelViewSearch.PageSize,
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
