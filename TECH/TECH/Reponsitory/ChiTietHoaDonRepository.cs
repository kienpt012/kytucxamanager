using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IChiTietHoaDonRepository : IRepository<ChiTietHoaDon, int>
    {
       
    }

    public class ChiTietHoaDonRepository : EFRepository<ChiTietHoaDon, int>, IChiTietHoaDonRepository
    {
        public ChiTietHoaDonRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
