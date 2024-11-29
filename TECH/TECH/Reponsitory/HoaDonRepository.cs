using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IHoaDonRepository : IRepository<HoaDon, int>
    {
       
    }

    public class HoaDonRepository : EFRepository<HoaDon, int>, IHoaDonRepository
    {
        public HoaDonRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
