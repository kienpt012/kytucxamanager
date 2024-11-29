using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface ILoiPhamRepository : IRepository<LoiPham, int>
    {
       
    }

    public class LoiPhamRepository : EFRepository<LoiPham, int>, ILoiPhamRepository
    {
        public LoiPhamRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
