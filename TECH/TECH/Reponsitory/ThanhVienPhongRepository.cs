using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IThanhVienPhongRepository : IRepository<ThanhVienPhong, int>
    {
       
    }

    public class ThanhVienPhongRepository : EFRepository<ThanhVienPhong, int>, IThanhVienPhongRepository
    {
        public ThanhVienPhongRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
