using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IPhuongXaRepository : IRepository<PhuongXa, int>
    {
       
    }

    public class PhuongXaRepository : EFRepository<PhuongXa, int>, IPhuongXaRepository
    {
        public PhuongXaRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
