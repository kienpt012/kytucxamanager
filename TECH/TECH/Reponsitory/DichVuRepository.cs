using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IDichVuRepository : IRepository<DichVu, int>
    {
       
    }

    public class DichVuRepository : EFRepository<DichVu, int>, IDichVuRepository
    {
        public DichVuRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
