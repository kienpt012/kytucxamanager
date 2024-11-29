
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
    public interface ILoiPhamService
    {
        LoiPhamModelView GetByid(int id);
        int Add(LoiPhamModelView view);
        bool Update(LoiPhamModelView view);
        bool Deleted(int id);
        void Save();
    }

    public class LoiPhamService : ILoiPhamService
    {
        private readonly ILoiPhamRepository _loiPhamRepository;
        private IUnitOfWork _unitOfWork;
        public LoiPhamService(ILoiPhamRepository loiPhamRepository,
            IUnitOfWork unitOfWork)
        {
            _loiPhamRepository = loiPhamRepository;
            _unitOfWork = unitOfWork;
        }       
        public LoiPhamModelView GetByid(int id)
        {
            var data = _loiPhamRepository.FindAll(p => p.Id == id).FirstOrDefault();
            if (data != null)
            {
                var model = new LoiPhamModelView()
                {
                    Id = data.Id,
                    TenLoi = !string.IsNullOrEmpty(data.TenLoi) ? data.TenLoi:"",
                    TienPhat = data.TienPhat,
                    TienPhatStr = data.TienPhat.HasValue && data.TienPhat.Value > 0 ? data.TienPhat.Value.ToString("#,###") : "",
                    GhiChu = !string.IsNullOrEmpty(data.GhiChu) ? data.GhiChu : "",
                };
                return model;
            }
            return null;
        }
        public int Add(LoiPhamModelView view)
        {
            try
            {
                if (view != null)
                {
                    var loipham = new LoiPham
                    {
                        TenLoi = view.TenLoi,
                        TienPhat = view.TienPhat,
                        GhiChu = view.GhiChu                    
                    };
                    _loiPhamRepository.Add(loipham);
                    Save();
                    return loipham.Id;
                }
            }
            catch (Exception ex)
            {
            }
            return 0;
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public bool Update(LoiPhamModelView view)
        {
            try
            {
                var dataServer = _loiPhamRepository.FindById(view.Id);
                if (dataServer != null)
                {
                    dataServer.TenLoi = view.TenLoi;
                    dataServer.TienPhat = view.TienPhat;
                    dataServer.GhiChu = view.GhiChu;
                    _loiPhamRepository.Update(dataServer);
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
                var dataServer = _loiPhamRepository.FindById(id);
                if (dataServer != null)
                {
                    _loiPhamRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return false;
        }        
    }
}
