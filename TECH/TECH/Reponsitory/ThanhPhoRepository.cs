using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IThanhPhoRepository : IRepository<ThanhPho, int>
    {
       
    }

    public class ThanhPhoRepository : EFRepository<ThanhPho, int>, IThanhPhoRepository
    {
        public ThanhPhoRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
