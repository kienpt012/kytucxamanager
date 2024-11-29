using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IPhongRepository : IRepository<Phong, int>
    {
       
    }

    public class PhongRepository : EFRepository<Phong, int>, IPhongRepository
    {
        public PhongRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
