using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IHopDongRepository : IRepository<HopDong, int>
    {
       
    }

    public class HopDongRepository : EFRepository<HopDong, int>, IHopDongRepository
    {
        public HopDongRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
