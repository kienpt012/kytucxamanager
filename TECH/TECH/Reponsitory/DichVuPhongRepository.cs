using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IDichVuPhongRepository : IRepository<DichVuPhong, int>
    {
       
    }

    public class DichVuPhongRepository : EFRepository<DichVuPhong, int>, IDichVuPhongRepository
    {
        public DichVuPhongRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
