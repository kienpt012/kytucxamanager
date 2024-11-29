using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IKhachHangRepository : IRepository<KhachHang, int>
    {
       
    }

    public class KhachHangRepository : EFRepository<KhachHang, int>, IKhachHangRepository
    {
        public KhachHangRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
