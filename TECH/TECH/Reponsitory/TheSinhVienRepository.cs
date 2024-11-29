using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface ITheSinhVienRepository : IRepository<TheSinhVien, int>
    {
       
    }

    public class TheSinhVienRepository : EFRepository<TheSinhVien, int>, ITheSinhVienRepository
    {
        public TheSinhVienRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
