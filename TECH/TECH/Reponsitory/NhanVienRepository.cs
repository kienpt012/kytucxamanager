using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface INhanVienRepository : IRepository<NhanVien, int>
    {
       
    }

    public class NhanVienRepository : EFRepository<NhanVien, int>, INhanVienRepository
    {
        public NhanVienRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
